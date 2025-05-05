using Fron.Application.Abstractions.Identity;
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

    public string GenerateToken(string? userName)
    {
        var claims = new List<Claim>()
        {
            new (JwtClaimNames.USER_NAME, userName!),
        };

        //foreach (var role in roles)
        //{
        //    claims.Add(new Claim(type: ClaimTypes.Role, role));
        //}

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