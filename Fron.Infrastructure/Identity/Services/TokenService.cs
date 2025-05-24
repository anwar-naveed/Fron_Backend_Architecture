using Fron.Application.Abstractions.Identity;
using Fron.Domain.AuthEntities;
using Fron.Domain.Configuration;
using Fron.Domain.Constants;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fron.Infrastructure.Identity.Services;
public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly AuthenticationConfiguration _authenticationConfiguration;

    public TokenService(IOptions<AuthenticationConfiguration> authenticationConfiguration)
    {
        _authenticationConfiguration = authenticationConfiguration.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationConfiguration.SecretKey));
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new (JwtClaimNames.USER_NAME, user.Username!),
            new (JwtClaimNames.ROLES, string.Empty), //so that roles claim would be an array everytime as one role will be always added when creating user
        };

        if (user.UserRoles != null && user.UserRoles.Count > 0)
        {
            foreach (var userRole in user.UserRoles)
            {
                //claims.Add(new Claim(type: ClaimTypes.Role, userRole.Role.Name!)); //claim name was too big
                claims.Add(new Claim(type: JwtClaimNames.ROLES, userRole.Role.Name!));
            }
        }

        var creds = new SigningCredentials(_key, algorithm: SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                   issuer: _authenticationConfiguration.Issuer,
                   audience: _authenticationConfiguration.Audience,
                   claims: claims,
                   expires: DateTime.Now.AddDays(MagicNumbers.TOKEN_EXPIRY_DAYS),
                   signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}