using Fron.Domain.AuthEntities;

namespace Fron.Application.Abstractions.Persistence;
public interface IRoleRepository
{
    Task<Role> CreateRoleAsync(Role entity);
    Task DeleteRoleAsync(Role entity);
    Task<Role?> GetByIdAsync(long id);
    Task<Role> UpdateRoleAsync(Role entity);
}