using Fron.Application.Abstractions.Persistence;
using Fron.Domain.AuthEntities;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Repositories;
public class UserRepository : AuthRepository, IUserRepository
{
    public UserRepository(DataDbContext context,
        AuthDbContext authContext) : base(context, authContext)
    {
    }

    public async Task<User> CreateUserAsync(User entity)
    {
        //var user = 
        var user =  await _authContext.User.AddAsync(entity);
        //UserRole userRole = new UserRole()
        //{
        //    RoleId = user.Entity.

        //}

        await _authContext.SaveChangesAsync();

        return user.Entity;
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
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteUserAsync(User entity)
    {
        _authContext.User.Remove(entity);
        await _authContext.SaveChangesAsync();
    }
}
