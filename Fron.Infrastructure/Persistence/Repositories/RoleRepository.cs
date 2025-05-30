﻿using Fron.Application.Abstractions.Persistence;
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
            .Where(e => e.Id == id && e.IsActive == true)
            .Include(x => x.UserRoles)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteRoleAsync(Role entity)
    {
        _authContext.Role.Remove(entity);
        await _authContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<GetAllRolesResponseDto>> GetAllRolesAsync()
    {
        return await _authContext.Role
            .Where(x => x.IsActive == true)
            .Select(x => new GetAllRolesResponseDto(
            x.Id,
            x.Name,
            x.IsActive,
            x.CreatedOn,
            x.ModifiedOn
            ))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task BulkInsertRolesAsync(IEnumerable<Role> roles)
    {
        await _authContext.AddRangeAsync(roles);
        await _authContext.SaveChangesAsync();
    }
}
