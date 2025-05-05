namespace Fron.Application.Abstractions.Identity;

public interface ITokenService
{
    string GenerateToken(string? userName);
}