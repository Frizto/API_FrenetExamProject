using ApplicationLayer.CQRS.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ApplicationLayer.Extensions;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.DTOs;
using InfrastructureLayer.DataAccess;
using DomainLayer.Enums;
using DomainLayer.Models;
using Microsoft.Extensions.Logging;
using NLog;
using System.Diagnostics;

namespace InfrastructureLayer.Handlers.User;
sealed class CreateUserHandler(UserManager<AppUser> userManager,
    AppDbContext appDbContext,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<CreateUserCommand, ServiceResponse>
{
    // 0.a Create logger instances
    private static readonly Logger CreateLogger = LogManager.GetLogger("CreateLogger");
    private static readonly Logger ErrorLogger = LogManager.GetLogger("ErrorLogger");

    // 0.b Generate a unique transaction ID using a GUID
    string transactionId = Guid.NewGuid().ToString();

    public async Task<ServiceResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        using (ScopeContext.PushProperty("TransactionId", transactionId));
        using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 0. Create the AspNetUser for credentials.
            var aspUser = new AppUser
            {
                UserName = command.Email,
                PasswordHash = command.Password,
                Name = command.Name,
                Email = command.Email,
                NormalizedEmail = command.Email.ToUpper(),
                UserType = command.UserRole.ToString()
            };

            var createAspUserResult = await userManager.CreateAsync(aspUser, command.Password);
            if (!createAspUserResult.Succeeded)
            {
                transaction.Rollback();
                return new ServiceResponse(false, "Failed when creating the Asp User", DateTime.UtcNow);
            }

            // 1. Create its claims.
            var claims = CreateClaims(command);
            var claimsResult = await userManager.AddClaimsAsync(aspUser, claims);
            if (!claimsResult.Succeeded)
            {
                transaction.Rollback();
                return new ServiceResponse(false, "Failed when creating the User Claims", DateTime.UtcNow);
            }

            // 2. Create the client for the Db user.
            var dbUser = new Client
            {
                Name = command.Name,
                Email = command.Email,
                Phone = command.Phone,
                AspNetUserId = aspUser.Id
            };

            var dbUserResult = await appDbContext.Clients.AddAsync(dbUser, cancellationToken);
            if (dbUserResult.State != EntityState.Added)
            {
                transaction.Rollback();
                return new ServiceResponse(false, "Failed when creating the Db User", DateTime.UtcNow);
            }

            
            // 3. Save the changes.
            var resultDb = await appDbContext.SaveChangesAsync(cancellationToken);
            if (resultDb == 0)
            {
                transaction.Rollback();
                return new ServiceResponse(false, "Failed when saving user creation to Db", DateTime.UtcNow);
            } 
            
            // 4. Create the Log.
            CreateLogger.Info("User Created Successfully");

            // If all operations succeed, commit the transaction.
            transaction.Commit();
            return new ServiceResponse(true, "User Created Successfully", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            // If an unexpected error occurs, roll back the transaction.
            transaction.Rollback();
            Trace.WriteLine(ex.Message);
            return new ServiceResponse(false, "Error when creating the User", DateTime.UtcNow);
        }
    }

    private static Claim[] CreateClaims(CreateUserCommand command)
    {
        Claim[] claims = [];

        if (command.UserRole.Equals(AppUserTypeEnum.Admin))
        {
            claims =
            [
                new Claim("Name", command.Name),
                new Claim(ClaimTypes.Email, command.Email),
                new Claim(ClaimTypes.Role, nameof(AppUserTypeEnum.Admin)),
            ];
        }
        else if (command.UserRole.Equals(AppUserTypeEnum.Client))
        {
            claims =
            [
                new Claim("Name", command.Name),
                new Claim(ClaimTypes.Email, command.Email),
                new Claim(ClaimTypes.Role, nameof(AppUserTypeEnum.Client)),
            ];
        }

        return claims;
    }
}
