using Fron.Domain.AuthEntities;

namespace Fron.Application.Abstractions.Identity;

public interface ITokenService
{
    string GenerateToken(User user);
}