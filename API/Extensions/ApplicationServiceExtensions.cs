using Application.Interfaces;
using Application.Profiles;
using Application.Repositories;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServiceExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        });

        services.AddSignalR();
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });
        services.AddCors();
        services.AddAutoMapper(typeof(AccountProfile).Assembly);
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IServerRepository, ServerRepository>();
        services.AddScoped<IInviteRepository, InviteRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Cache");
            options.InstanceName = "RedisInstance";
        });
        services.AddSingleton<IRedisService, RedisService>();
        return services;
    }
}