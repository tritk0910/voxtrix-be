using API.Extensions;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
// Add services to the container.
builder.Services
    .AddIdentityService()
    .AddApplicationServiceExtensions();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();