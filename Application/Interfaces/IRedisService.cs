namespace Application.Interfaces;

public interface IRedisService
{
    Task SetOtpAsync(string userId, string otp, TimeSpan expiry);
    Task<string> GetOtpAsync(string userId);
    Task DeleteOtpAsync(string userId);
    Task SetOtpVerifiedAsync(string userId, TimeSpan expiry);
    Task<bool> IsOtpVerifiedAsync(string userId);
    Task DeleteOtpVerifiedAsync(string userId);
}