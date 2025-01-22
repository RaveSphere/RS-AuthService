using Application.Interfaces;
using Application.Services;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddScoped<IHashingService, HashingService>()
                .AddScoped<ITokenService, TokenService>()
                .Configure<JwtSettingsModel>(configuration.GetSection("JwtSettings"));
        }
    }
}
