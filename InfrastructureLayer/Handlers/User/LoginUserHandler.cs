using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.User;
using ApplicationLayer.Extensions;
using ApplicationLayer.Helpers;
using InfrastructureLayer.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace InfrastructureLayer.Handlers.User;
public sealed class LoginUserHandler(UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IConfiguration configuration) : ICommandHandler<LoginUserCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            // 0. Check if the user already exists.
            var user = await userManager.FindByEmailAsync(command.Email!);
            if (user == null) return new TokenResultDTO(false);

            // 1. Check if the user credentials are correct and setup lockout.
            var result = await signInManager.CheckPasswordSignInAsync(user, command.Password!, true);
            if (!result.Succeeded) return new TokenResultDTO(false);

            // 2. Get the user role to pass to the Token.
            var userRole = userManager.GetClaimsAsync(user).Result.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // 3. Generate the JWT Token.
            var tokenGen = new JWTGenerator(configuration);
            var logInToken = await tokenGen.GenerateJWTTokenAsync(user, userRole!);

            return new TokenResultDTO(true, logInToken);
        }
    }
