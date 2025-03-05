using System.Reflection;
using System.Text.Json.Serialization;
using Application.Interfaces;
using Application.Profiles;
using Application.Repositories;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence;
using Swashbuckle.AspNetCore.Filters;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServiceExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Voxtrix API", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            c.AddSecurityDefinition("JWT Token", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            c.OperationFilter<SecurityRequirementsOperationFilter>();
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