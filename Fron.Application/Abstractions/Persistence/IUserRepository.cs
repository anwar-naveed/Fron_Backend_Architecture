using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.User;

namespace Fron.Application.Abstractions.Persistence;
public interface IUserRepository
{
    Task<User> CreateUserAsync(User us, Role role);
    Task DeleteUserAsync(User entity);
    Task<User?> GetByIdAsync(long id);
    Task<User> UpdateUserAsync(User entity);
    Task<User?> GetUserAsync(string userName, string password);
    Task<IEnumerable<GetAllUsersResponseDto>> GetAllUsersAsync();
}