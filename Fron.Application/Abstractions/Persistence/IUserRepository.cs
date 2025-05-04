using Fron.Domain.AuthEntities;

namespace Fron.Application.Abstractions.Persistence;
public interface IUserRepository
{
    Task<User> CreateUserAsync(User us, Role role);
    Task DeleteUserAsync(User entity);
    Task<User?> GetByIdAsync(long id);
    Task<User> UpdateUserAsync(User entity);
}