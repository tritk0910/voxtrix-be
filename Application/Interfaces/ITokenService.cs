using Application.DTOs.Accounts;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
    string CreateRefreshToken(AppUser user);
    RefreshTokenDto RefreshToken(string refreshToken);
    bool ValidateToken(string token);
    Task<bool> Logout(string refreshToken);
    Task<bool> LogoutAllDevices(string userId);
}