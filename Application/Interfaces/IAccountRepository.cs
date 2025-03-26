namespace Application.Interfaces;

public interface IAccountRepository
{
    Task<bool> Logout(string refreshToken);
    Task<bool> LogoutAllDevices(string userId);
}