using Fron.Application.Abstractions.Persistence;
using Fron.Infrastructure.Persistence.Contexts;

namespace Fron.Infrastructure.Persistence.Repositories;
public class AuthRepository : BaseRepository, IAuthRepository
{
    protected AuthDbContext _authContext;
    public AuthRepository(DataDbContext context,
        AuthDbContext authContext) : base(context)
    {
        _authContext = authContext;
    }
}
