﻿using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.DTOs;
using ApplicationLayer.Extensions;
using DomainLayer.Enums;
using DomainLayer.Models;
using InfrastructureLayer.DataAccess;
using InfrastructureLayer.Loggers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Security.Claims;

namespace InfrastructureLayer.Handlers.User;
public sealed class CreateUserHandler(UserManager<AppUser> userManager,
    AppDbContext appDbContext) : ICommandHandler<CreateUserCommand, ServiceResponse>
{
    public async Task<ServiceResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            // 1. Create the AspNetUser for credentials.
            var aspUser = new AppUser
            {
                UserName = command.Email,
                PasswordHash = command.Password,
                Name = command.Name,
                Email = command.Email,
                NormalizedEmail = command.Email!.ToUpper(),
                UserType = command.UserRole.ToString()
            };

            var createAspUserResult = await userManager.CreateAsync(aspUser, command.Password!);
            if (!createAspUserResult.Succeeded)
            {
                throw new Exception("Failed when creating the Asp User");
            }

            // 1a. Create its claims.
            var claims = CreateClaims(command);
            var claimsResult = await userManager.AddClaimsAsync(aspUser, claims);
            if (!claimsResult.Succeeded)
            {
                throw new Exception("Failed when creating the User Claims");
            }

            // 2. Create the client for the Db user.         
            var clientAddress = new Address
            {
                Street = command.Address!.Street,
                City = command.Address!.City,
                State = command.Address!.State,
                ZipCode = command.Address!.ZipCode,
                Client = new Client
                {
                    Name = command.Name,
                    Email = command.Email,
                    Phone = command.Phone,
                    AspNetUserId = aspUser.Id
                }
            };

            var addressResult = await appDbContext.Addresses.AddAsync(clientAddress, cancellationToken);
            if (addressResult.State != EntityState.Added)
            {
                throw new Exception("Failed when creating the Client");
            }

            // 3a. Save the changes.
            var resultDb = await appDbContext.SaveChangesAsync(cancellationToken);
            if (resultDb == 0)
            {
                throw new Exception("Failed when saving user creation to Db");
            }

            // 3b. If all operations succeed, commit the transaction.
            await transaction.CommitAsync(cancellationToken);

            // 4. Create the Log.
            if (appDbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                using (ScopeContext.PushProperty("TransactionId", Guid.NewGuid().ToString()))
                {
                    ScopeContext.PushProperty("EntityId", aspUser.Id);
                    CustomLoggers.CreateLogger.Info("User Created Successfully");
                }
            }

            return new ServiceResponse(true, "User Created Successfully", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            // If an unexpected error occurs, roll back the transaction.
            transaction.Rollback();
            if (appDbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                CustomLoggers.ErrorLogger.Error(ex, ex.Message);
            }
            return new ServiceResponse(false, ex.Message, DateTime.UtcNow);
        }
    }

    // 1b. Create the claims based on the user role.
    private static Claim[] CreateClaims(CreateUserCommand command)
    {
        Claim[] claims = [];

        if (command.UserRole.Equals(AppUserTypeEnum.Admin))
        {
            claims =
            [
                new Claim("Name", command.Name!),
                new Claim(ClaimTypes.Email, command.Email),
                new Claim(ClaimTypes.Role, nameof(AppUserTypeEnum.Admin)),
            ];
        }
        else if (command.UserRole.Equals(AppUserTypeEnum.Client))
        {
            claims =
            [
                new Claim("Name", command.Name!),
                new Claim(ClaimTypes.Email, command.Email!),
                new Claim(ClaimTypes.Role, nameof(AppUserTypeEnum.Client)),
            ];
        }

        return claims;
    }
}
