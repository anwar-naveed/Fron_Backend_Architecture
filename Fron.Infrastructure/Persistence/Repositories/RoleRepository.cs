using Fron.Application.Abstractions.Persistence;
using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.Role;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Repositories;
public class RoleRepository : AuthRepository, IRoleRepository
{
    public RoleRepository(DataDbContext context,
        AuthDbContext authContext) : base(context, authContext)
    {
    }

    public async Task<Role> CreateRoleAsync(Role entity)
    {
        var role = await _authContext.Role.AddAsync(entity);
        await _authContext.SaveChangesAsync();
        return role.Entity;
    }

    public async Task<Role> UpdateRoleAsync(Role entity)
    {
        var role = _authContext.Role.Update(entity);
        await _authContext.SaveChangesAsync();
        return role.Entity;
    }

    public async Task<Role?> GetByIdAsync(long id)
    {
        return await _authContext.Role
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteRoleAsync(Role entity)
    {
        _authContext.Role.Remove(entity);
        await _authContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<GetAllRolesResponseDto>> GetAllRolesAsync()
    {
        return await _authContext.Role.Select(x => new GetAllRolesResponseDto(
            x.Id,
            x.Name
            ))
            .AsNoTracking()
            .ToListAsync();
    }
}
