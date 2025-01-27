using StackExchange.Redis;
using Application.Interfaces;

namespace Application.Services;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisService()
    {
        _redis = ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING"));
        _database = _redis.GetDatabase();
    }

    public async Task SetOtpAsync(string key, string otp, TimeSpan expiration)
    {
        await _database.StringSetAsync(key, otp, expiration);
    }

    public async Task<string> GetOtpAsync(string key)
    {
        return await _database.StringGetAsync(key);
    }

    public async Task DeleteOtpAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}