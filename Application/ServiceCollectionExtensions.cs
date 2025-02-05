using Application.Interfaces;
using Application.Services;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public async static Task<IServiceCollection> AddApplicationServices(this IServiceCollection services, IConfiguration configuration, string? environment)
        {
            JwtSettingsModel? jwtSettingsModel = new()
            {
                Key = string.Empty,
                Issuer = string.Empty,
                Audience = string.Empty
            };

            if (environment == "Production")
            {
                var vaultService = services.BuildServiceProvider().GetRequiredService<IVaultService>();
                string? jwtSettingsJson = await vaultService.GetSecretJsonAsync("userService/connectionStrings");

                if (!string.IsNullOrEmpty(jwtSettingsJson))
                {
                    jwtSettingsModel = JsonSerializer.Deserialize<JwtSettingsModel>(jwtSettingsJson)
                        ?? throw new InvalidOperationException("JWT settings could not be loaded from Vault");
                }
            }
            else
            {
                jwtSettingsModel = configuration.GetSection("JwtSettings").Get<JwtSettingsModel>()
                    ?? throw new InvalidOperationException("JWT settings could not be loaded from local secret");
            }

            return services
                .AddScoped<IHashingService, HashingService>()
                .AddScoped<ITokenService, TokenService>()
                .Configure<JwtSettingsModel>(options =>
                {
                    options.Key = jwtSettingsModel.Key;
                    options.Issuer = jwtSettingsModel.Issuer;
                    options.Audience = jwtSettingsModel.Audience;
                });
        }
    }
}
