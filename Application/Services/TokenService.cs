using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs.Accounts;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace Application.Services;

public class TokenService(IConfiguration config, DataContext context) : ITokenService
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private const int AccessTokenExpirationMinutes = 5;
    private const int RefreshTokenExpirationDays = 90;

    public string CreateToken(AppUser user) =>
        GenerateToken(user, config["TokenKey"], TimeSpan.FromMinutes(AccessTokenExpirationMinutes));

    public string CreateRefreshToken(AppUser user)
    {
        var newRefreshToken = GenerateToken(user, config["RefreshTokenKey"], TimeSpan.FromDays(RefreshTokenExpirationDays));
        context.RefreshTokens.Add(new RefreshToken { Token = newRefreshToken, Expires = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays), UserId = user.Id });
        context.SaveChanges();
        return newRefreshToken;
    }

    public RefreshTokenDto RefreshToken(string refreshToken)
    {
        var existingToken = context.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);
        if (existingToken == null || existingToken.Expires < DateTime.UtcNow)
            return null;

        var currentRefreshToken = _tokenHandler.ReadJwtToken(refreshToken);
        var userId = currentRefreshToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

        var dbRefreshToken = _tokenHandler.ReadJwtToken(existingToken.Token);
        var dbUserId = dbRefreshToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

        if (userId != dbUserId) return null;

        var user = context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null) return null;

        var newToken = GenerateToken(user, config["TokenKey"], TimeSpan.FromMinutes(AccessTokenExpirationMinutes));
        var newRefreshToken = GenerateToken(user, config["RefreshTokenKey"],
            existingToken.Expires - DateTime.UtcNow);

        context.RefreshTokens.Remove(existingToken);
        context.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            Expires = existingToken.Expires,
            UserId = userId
        });
        context.SaveChanges();

        return new RefreshTokenDto
        {
            Token = newToken,
            RefreshToken = newRefreshToken
        };
    }

    public bool ValidateToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = CreateSecurityKey(config["TokenKey"]),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            _tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateToken(AppUser user, string key, TimeSpan expiration)
    {
        var claims = CreateClaims(user);
        var signingCredentials = new SigningCredentials(
            CreateSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(expiration),
            SigningCredentials = signingCredentials
        };

        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }

    private static List<Claim> CreateClaims(AppUser user) =>
    [
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email)
    ];

    private static SymmetricSecurityKey CreateSecurityKey(string key) =>
        new(Encoding.UTF8.GetBytes(key));

    public async Task<bool> Logout(string refreshToken)
    {
        var existingToken = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (existingToken == null) return false;

        context.RefreshTokens.Remove(existingToken);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LogoutAllDevices(string userId)
    {
        var existingTokens = await context.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
        if (existingTokens.Count == 0) return false;

        context.RefreshTokens.RemoveRange(existingTokens);
        await context.SaveChangesAsync();

        return true;
    }
}
