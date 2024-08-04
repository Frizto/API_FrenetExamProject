using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.User;
using ApplicationLayer.Extensions;
using DomainLayer.Enums;
using InfrastructureLayer.DataAccess;
using InfrastructureLayer.Handlers.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Text;

namespace InfrastructureLayer.DependencyInjection;
public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Adds the DbContexts to the services container
        services.AddDbContext<AppDbContext>(ServiceLifetime.Scoped);
        services.AddDbContext<LogsDbContext>(ServiceLifetime.Scoped);

        // 2. Adds the Identity services to the services container
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddRoles<IdentityRole>();

        // 3. Adds authentication structure using JWT Json Web Token
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration[nameof(JwtTokenConfigEnum.JwtFEIssuer)],
                ValidAudience = configuration[nameof(JwtTokenConfigEnum.JwtFEAudience)],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[nameof(JwtTokenConfigEnum.JwtFEKey)]!))
            };
        });

        // 4. Adds the CQRS services to the container
        services.AddScoped<ICommandHandler<CreateUserCommand, ServiceResponse>, CreateUserHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand, ServiceResponse>, UpdateUserHandler>();
        services.AddScoped<ICommandHandler<DeleteUserCommand, ServiceResponse>, DeleteUserHandler>();
        services.AddScoped<IQueryHandler<ReadUserQuery, ReadUserDTO>, ReadUserHandler>();
        services.AddScoped<ICommandHandler<LoginUserCommand, ServiceResponse>, LoginUserHandler>();

        return services;
    }
}