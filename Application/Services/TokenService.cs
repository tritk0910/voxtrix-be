using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs.Accounts;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace Application.Services;

public class TokenService(IConfiguration config, DataContext context) : ITokenService
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private const int AccessTokenExpiration = 5;
    private const int RefreshTokenExpiration = 90;

    public Task<string> CreateTokenAsync(AppUser user)
    {
        return Task.FromResult(GenerateToken(user, config["TokenKey"], TimeSpan.FromMinutes(AccessTokenExpiration)));
    }

    public async Task<RefreshTokenCookieResponse> CreateRefreshTokenAsync(AppUser user)
    {
        var expirationDate = DateTime.UtcNow.AddDays(RefreshTokenExpiration);
        var newRefreshToken = GenerateToken(
            user,
            config["RefreshTokenKey"],
            TimeSpan.FromDays(RefreshTokenExpiration)
        );

        var refreshToken = new RefreshToken
        {
            Token = newRefreshToken,
            Expires = expirationDate,
            UserId = user.Id
        };

        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        return new RefreshTokenCookieResponse
        {
            RefreshToken = newRefreshToken,
            Expires = expirationDate
        };
    }

    public async Task<RefreshTokenCookieResponse> RefreshTokenAsync(string refreshToken)
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

        var newToken = GenerateToken(user, config["TokenKey"], TimeSpan.FromMinutes(AccessTokenExpiration));
        var newRefreshToken = GenerateToken(user, config["RefreshTokenKey"],
            existingToken.Expires - DateTime.UtcNow);

        context.RefreshTokens.Remove(existingToken);
        context.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            Expires = existingToken.Expires,
            UserId = userId
        });
        await context.SaveChangesAsync();

        return new RefreshTokenCookieResponse
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            Expires = existingToken.Expires
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
}
