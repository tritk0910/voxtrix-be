using StackExchange.Redis;
using Application.Interfaces;

namespace Application.Services;

public class RedisService : IRedisService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public RedisService()
    {
        var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
        var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");
        var redisUser = Environment.GetEnvironmentVariable("REDIS_USER");
        var redisPassword = Environment.GetEnvironmentVariable("REDIS_PASSWORD");

        var configurationOptions = new ConfigurationOptions
        {
            EndPoints = { $"{redisHost}:{redisPort}" },
            User = redisUser,
            Password = redisPassword,
            Ssl = true,
            AbortOnConnectFail = false
        };

        _redis = ConnectionMultiplexer.Connect(configurationOptions);
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