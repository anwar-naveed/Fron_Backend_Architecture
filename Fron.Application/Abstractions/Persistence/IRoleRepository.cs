using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.Role;

namespace Fron.Application.Abstractions.Persistence;
public interface IRoleRepository
{
    Task<Role> CreateRoleAsync(Role entity);
    Task DeleteRoleAsync(Role entity);
    Task<Role?> GetByIdAsync(long id);
    Task<Role> UpdateRoleAsync(Role entity);
    Task<IEnumerable<GetAllRolesResponseDto>> GetAllRolesAsync();
}