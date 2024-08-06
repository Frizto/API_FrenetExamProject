using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace ApplicationLayer.DependencyInjection;
public static class ServiceContainer
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // 0. Adds the Jwt Generation Token service
        services.AddScoped<JwtSecurityTokenHandler>();

        // 1. Adds the repo services
        // We can register here the future services
        return services;
    }
}