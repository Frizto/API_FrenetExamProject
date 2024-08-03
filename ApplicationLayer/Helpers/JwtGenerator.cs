using ApplicationLayer.Extensions;
using DomainLayer.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApplicationLayer.Helpers;
public class JWTGenerator(IConfiguration configuration)
{
    public async Task<string> GenerateJWTTokenAsync(AppUser user, string role)
    {
        // Implementation of JWT token generation
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[nameof(JwtTokenConfigEnum.JwtFEKey)]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var userClaims = new[]
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: configuration[nameof(JwtTokenConfigEnum.JwtFEIssuer)],
            audience: configuration[nameof(JwtTokenConfigEnum.JwtFEAudience)],
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return await Task.FromResult(tokenHandler.WriteToken(token));
    }
}
