using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Commands;
using ApplicationLayer.CQRS.Shipment.Queries;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Shipment;
using ApplicationLayer.DTOs.User;
using ApplicationLayer.Extensions;
using DomainLayer.Enums;
using InfrastructureLayer.DataAccess;
using InfrastructureLayer.Handlers.Shipment;
using InfrastructureLayer.Handlers.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        var dbConnectionString = Environment.GetEnvironmentVariable("ASPNETCORE_FRENETEXAM_DEV");
        var logsConnectionString = Environment.GetEnvironmentVariable("ASPNETCORE_FRENETEXAM_DEV");

        services.AddDbContext<AppDbContext>(options => 
        options.UseSqlServer(dbConnectionString), ServiceLifetime.Scoped);
        services.AddDbContext<LogsDbContext>(options => 
        options.UseSqlServer(logsConnectionString), ServiceLifetime.Scoped);

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[nameof(JwtTokenConfigEnum.JwtFEKey)])!)
            };
        });

        // 4. Adds authorization policies for admin and user roles
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy =>
            {
                policy.RequireRole("Admin");
            }).AddPolicy("ClientOnly", policy =>
            {
                policy.RequireRole("Client");
            });

        // 5. Adds the CQRS services to the container
        // 5a. User Handlers
        services.AddScoped<ICommandHandler<CreateUserCommand, ServiceResponse>, CreateUserHandler>();
        services.AddScoped<ICommandHandler<UpdateUserCommand, ServiceResponse>, UpdateUserHandler>();
        services.AddScoped<ICommandHandler<DeleteUserCommand, ServiceResponse>, DeleteUserHandler>();
        services.AddScoped<IQueryHandler<ReadUserQuery, ReadUserDTO>, ReadUserHandler>();
        services.AddScoped<ICommandHandler<LoginUserCommand, ServiceResponse>, LoginUserHandler>();
        services.AddScoped<IQueryHandler<RefreshUserTokenQuery, TokenResultDTO>, RefreshUserTokenHandler>();
        // 5b. Shipment Handlers
        services.AddScoped<ICommandHandler<CreateShipmentCommand, ServiceResponse>, CreateShipmentHandler>();
        services.AddScoped<ICommandHandler<UpdateShipmentCommand, ServiceResponse>, UpdateShipmentHandler>();
        services.AddScoped<ICommandHandler<DeleteShipmentCommand, ServiceResponse>, DeleteShipmentHandler>();
        services.AddScoped<IQueryHandler<ReadShipmentQuery, ReadShipmentDTO>, ReadShipmentHandler>();
        services.AddScoped<IQueryHandler<ShipmentPricingQuery, ShipmentPricingDTO>, ShipmentPricingHandler>();

        return services;
    }
}
