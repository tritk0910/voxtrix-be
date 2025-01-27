using Application.Interfaces;
using Application.Profiles;
using Application.Repositories;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServiceExtensions(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("DEFAULT_CONNECTIONS"));
        });
        services.AddCors();
        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IRedisService, RedisService>();
        return services;
    }
}