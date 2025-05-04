using Fron.Infrastructure.Persistence.Contexts;

namespace Fron.Infrastructure.Persistence.Repositories;
public abstract class BaseRepository
{
    protected DataDbContext _context;

    protected BaseRepository(DataDbContext context)
    {
        _context = context;
    }
}

