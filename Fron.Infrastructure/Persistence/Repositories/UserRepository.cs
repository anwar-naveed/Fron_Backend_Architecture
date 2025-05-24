using Fron.Application.Abstractions.Persistence;
using Fron.Application.Utility;
using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.User;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Repositories;
public class UserRepository : AuthRepository, IUserRepository
{
    public UserRepository(DataDbContext context,
        AuthDbContext authContext) : base(context, authContext)
    {
    }

    public async Task<User> CreateUserAsync(User us, Role role)
    {

        var entity = new UserRole();
        entity.User = us;
        entity.Role = role;
        entity.ModifiedDate = DateTime.UtcNow;

        var userRole =  await _authContext.UserRoles.AddAsync(entity);

        await _authContext.SaveChangesAsync();

        return userRole.Entity.User;
    }

    public async Task<User> AddUserRoleAsync(User us, Role role)
    {

        var entity = new UserRole();
        entity.User = us;
        entity.Role = role;
        entity.ModifiedDate = DateTime.UtcNow;

        var userRole = await _authContext.UserRoles.AddAsync(entity);

        await _authContext.SaveChangesAsync();

        return userRole.Entity.User;
    }

    public async Task<User> UpdateUserAsync(User entity)
    {
        var user = _authContext.User.Update(entity);
        await _authContext.SaveChangesAsync();
        return user.Entity;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _authContext.User
            .Where(e => e.Id == id && e.IsActive == true)
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteUserAsync(User entity)
    {
        _authContext.User.Remove(entity);
        await _authContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserAsync(string userName, string password)
        => await _authContext.User
        .Where(e => e.Username == userName &&
        e.Password == password && e.IsActive == true)
        .Include(x => x.UserRoles)
        .ThenInclude(x => x.Role)
        .FirstOrDefaultAsync();

    public async Task<IEnumerable<GetAllUsersResponseDto>> GetAllUsersAsync()
    {
        return await _authContext.User
            .Where(x => x.IsActive == true)
            .Select(x => new GetAllUsersResponseDto(
                x.Id,
                Helper.Base64Encode(x.Name!),
                Helper.Base64Encode(x.Username!),
                x.IsActive,
                x.CreatedOn,
                x.ModifiedOn))
            .AsNoTracking()
            .ToListAsync();
    }
}
