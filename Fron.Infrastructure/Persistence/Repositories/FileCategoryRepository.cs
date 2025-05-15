using Fron.Application.Abstractions.Persistence;
using Fron.Domain.Dto.FileCategory;
using Fron.Domain.Entities;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Repositories;
public class FileCategoryRepository : BaseRepository, IFileCategoryRepository
{
    public FileCategoryRepository(DataDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<FileCategory> CreateFileCategoryAsync(FileCategory entity)
    {
        var fileCategory = await _context.FileCategory.AddAsync(entity);
        await _context.SaveChangesAsync();
        return fileCategory.Entity;
    }

    public async Task<FileCategory> UpdateFileCategoryAsync(FileCategory entity)
    {
        var fileCategory = _context.FileCategory.Update(entity);
        await _context.SaveChangesAsync();
        return fileCategory.Entity;
    }

    public async Task<FileCategory?> GetByIdAsync(int id)
    {
        return await _context.FileCategory
            .Where(e => e.Id == id && e.IsActive == true)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<GetAllFileCategoryResponseDto>> GetAllFileCategoriesAsync()
    {
        return await _context.FileCategory
            .Where(x => x.IsActive == true)
            .Select(x => new GetAllFileCategoryResponseDto(
                x.Id,
                x.Name,
                x.FileCategoryEnum,
                x.IsActive,
                x.CreatedOn,
                x.ModifiedOn))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task DeleteFileCategoryAsync(FileCategory entity)
    {
        _context.FileCategory.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
