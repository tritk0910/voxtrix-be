using Microsoft.Extensions.Caching.Distributed;
using Application.Interfaces;

namespace Application.Services;

public class RedisService(IDistributedCache cache) : IRedisService
{
    public async Task SetOtpAsync(string key, string otp, TimeSpan expiration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        await cache.SetStringAsync(key, otp, options);
    }

    public async Task<string> GetOtpAsync(string key)
    {
        return await cache.GetStringAsync(key);
    }

    public async Task DeleteOtpAsync(string key)
    {
        await cache.RemoveAsync(key);
    }

    public async Task SetOtpVerifiedAsync(string userId, TimeSpan expiry)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry
        };
        await cache.SetStringAsync($"otp-verified:{userId}", "true", options);
    }

    public async Task<bool> IsOtpVerifiedAsync(string userId)
    {
        var result = await cache.GetStringAsync($"otp-verified:{userId}");
        return result == "true";
    }

    public async Task DeleteOtpVerifiedAsync(string userId)
    {
        await cache.RemoveAsync($"otp-verified:{userId}");
    }
}