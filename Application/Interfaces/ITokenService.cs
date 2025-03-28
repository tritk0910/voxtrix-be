using Application.DTOs.Accounts;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITokenService
{
    Task<string> CreateTokenAsync(AppUser user);
    Task<RefreshTokenCookieResponse> CreateRefreshTokenAsync(AppUser user);
    Task<RefreshTokenCookieResponse> RefreshTokenAsync(string refreshToken);
    bool ValidateToken(string token);
}