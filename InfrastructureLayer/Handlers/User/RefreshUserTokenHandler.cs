using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs.User;
using ApplicationLayer.Extensions;
using ApplicationLayer.Helpers;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InfrastructureLayer.Handlers.User;
public class RefreshUserTokenHandler(UserManager<AppUser> userManager,
    JwtSecurityTokenHandler jwtSecurityTokenHandler,
    IConfiguration configuration) : IQueryHandler<RefreshUserTokenQuery, TokenResultDTO>
{
    private static readonly Logger ErrorLogger = LogManager.GetLogger("ErrorLogger");
    public async Task<TokenResultDTO> Handle(RefreshUserTokenQuery query, CancellationToken cancellationToken)
    {
        try
        {
            // 0. Verify the current Token
            var currentToken = jwtSecurityTokenHandler.ReadJwtToken(query.Token);
            if (currentToken.ValidTo < DateTime.UtcNow)
            {
                return new TokenResultDTO(false, "Token is expired");
            }

            // 1. Verify the token signature
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration[nameof(JwtTokenConfigEnum.JwtFEIssuer)],
                ValidAudience = configuration[nameof(JwtTokenConfigEnum.JwtFEAudience)],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[nameof(JwtTokenConfigEnum.JwtFEKey)]!))
            };

            var principal = jwtSecurityTokenHandler.ValidateToken(query.Token, tokenValidationParameters, out SecurityToken validatedToken);

            // 2. Get the user and claims from the token
            var currentUserEmail = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var currentUserRole = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (currentUserEmail is null || currentUserRole is null)
            {
                return new TokenResultDTO(false, "Token is invalid");
            }

            // 3. Get the user from the database
            var aspUser = await userManager.FindByEmailAsync(currentUserEmail.Value);
            if (aspUser is null)
            {
                return new TokenResultDTO(false, "User not found");
            }

            // 4. Get admin user claims from database and checks it against the token
            var aspUserClaims = await userManager
                .GetClaimsAsync(aspUser);
            var aspUserEmailClaim = aspUserClaims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var aspUserRoleClaim = aspUserClaims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (aspUserClaims is null)
            {
                return new TokenResultDTO(false, "User not Found");
            }
            else if (aspUserEmailClaim?.Value != currentUserEmail.Value)
            {
                return new TokenResultDTO(false, "Invalid Token");
            }
            else if (aspUserRoleClaim?.Value != currentUserRole.Value)
            {
                return new TokenResultDTO(false, "Invalid Token");
            }

            // 5. Generate a new token
            var tokenGenerator = new JWTGenerator(configuration);
            var newToken = await tokenGenerator.GenerateJWTTokenAsync(aspUser, aspUserRoleClaim.Value);

            // 6. Return the new token
            return new TokenResultDTO(true, "Token Refreshed");
        }
        catch (Exception ex)
        {
            ErrorLogger.Error(ex, ex.Message);
            throw new Exception(ex.Message);
        }
    }

    public Task<IEnumerable<TokenResultDTO>> HandleListAsync(RefreshUserTokenQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
