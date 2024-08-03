using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.DTOs;
using ApplicationLayer.Extensions;
using DomainLayer.Enums;
using InfrastructureLayer.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Security.Claims;

namespace InfrastructureLayer.Handlers.User;

sealed class UpdateUserHandler(UserManager<AppUser> userManager,
    AppDbContext appDbContext) : ICommandHandler<UpdateUserCommand, ServiceResponse>
{
    private static readonly Logger UpdateLogger = LogManager.GetLogger("UpdateLogger");
    private static readonly Logger ErrorLogger = LogManager.GetLogger("ErrorLogger");

    public async Task<ServiceResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 0. Find the AspNetUser
            var aspUser = await userManager
                .FindByIdAsync(command.Id!)
                ?? throw new Exception("User not found");

            // 1. Update the AspNetUser properties
            aspUser.Name = command.Name;
            aspUser.Email = command.Email;
            aspUser.NormalizedEmail = command.Email.ToUpper();
            aspUser.UserName = command.Email;
            aspUser.NormalizedUserName = command.Email.ToUpper();
            aspUser.UserType = command.UserRole.ToString();

            var updateAspUserResult = await userManager.UpdateAsync(aspUser);
            if (!updateAspUserResult.Succeeded)
            {
                throw new Exception("Failed when updating the Asp User");
            }

            // 2. Update the claims
            var claims = CreateClaims(command);
            var existingClaims = await userManager.GetClaimsAsync(aspUser);
            var removeClaimsResult = await userManager.RemoveClaimsAsync(aspUser, existingClaims);
            if (!removeClaimsResult.Succeeded)
            {
                throw new Exception("Failed when removing the existing User Claims");
            }
            var addClaimsResult = await userManager.AddClaimsAsync(aspUser, claims);
            if (!addClaimsResult.Succeeded)
            {
                throw new Exception("Failed when adding the updated User Claims");
            }

            // 3. Update the client
            var client = await appDbContext.Clients
                .FirstOrDefaultAsync(c => c.AspNetUserId == command.Id, cancellationToken) 
                ?? throw new Exception("Client not found");
            client.Name = command.Name;
            client.Email = command.Email;
            client.Phone = command.Phone;

            // Save the changes
            var resultDb = await appDbContext.SaveChangesAsync(cancellationToken);
            if (resultDb == 0)
            {
                throw new Exception("Failed when saving user update to Db");
            }

            // Commit the transaction
            transaction.Commit();

            // Create the Log
            using (ScopeContext.PushProperty("TransactionId", Guid.NewGuid().ToString()))
            {
                ScopeContext.PushProperty("EntityId", aspUser.Id);
                UpdateLogger.Info("User Updated Successfully");
            }

            return new ServiceResponse(true, "User Updated Successfully", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            // Roll back the transaction if an unexpected error occurs
            transaction.Rollback();
            ErrorLogger.Error(ex, ex.Message);
            return new ServiceResponse(false, ex.Message, DateTime.UtcNow);
        }
    }

    private static Claim[] CreateClaims(UpdateUserCommand command)
    {
        Claim[] claims = [];

        if (command.UserRole.Equals(AppUserTypeEnum.Admin))
        {
            claims =
            [
                    new Claim("Name", command.Name),
                    new Claim(ClaimTypes.Email, command.Email),
                    new Claim(ClaimTypes.Role, nameof(AppUserTypeEnum.Admin))
            ];
        }
        else if (command.UserRole.Equals(AppUserTypeEnum.Client))
        {
            claims =
            [
                    new Claim("Name", command.Name),
                    new Claim(ClaimTypes.Email, command.Email),
                    new Claim(ClaimTypes.Role, nameof(AppUserTypeEnum.Client))
            ];
        }

        return claims;
    }
}

