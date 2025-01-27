namespace Application.Interfaces;

public interface IRedisService
{
    Task SetOtpAsync(string key, string otp, TimeSpan expiration);
    Task<string> GetOtpAsync(string key);
    Task DeleteOtpAsync(string key);
}