using Fron.Application.Abstractions.Persistence;
using Fron.Domain.Dto.TemplateExtension;
using Fron.Domain.Entities;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Repositories;
public class TemplateExtensionRepository : BaseRepository, ITemplateExtensionRepository
{
    public TemplateExtensionRepository(DataDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<TemplateExtension> CreateTemplateExtensionAsync(TemplateExtension entity)
    {
        var templateExtension = await _context.TemplateExtension.AddAsync(entity);
        await _context.SaveChangesAsync();
        return templateExtension.Entity;
    }

    public async Task<TemplateExtension> UpdateTemplateExtensionAsync(TemplateExtension entity)
    {
        var templateExtension = _context.TemplateExtension.Update(entity);
        await _context.SaveChangesAsync();
        return templateExtension.Entity;
    }

    public async Task<TemplateExtension?> GetByIdAsync(int? id)
    {
        return await _context.TemplateExtension
            .Where(e => e.Id == id && e.IsActive == true)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<GetAllTemplateExtensionsResponseDto>> GetAllTemplateExtensionsAsync()
    {
        return await _context.TemplateExtension
            .Where(x => x.IsActive == true)
            .Select(x => new GetAllTemplateExtensionsResponseDto(
                x.Id,
                x.Name,
                x.TemplateExtensionEnum,
                x.IsActive,
                x.CreatedOn,
                x.ModifiedOn))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task DeleteTemplateExtensionAsync(TemplateExtension entity)
    {
        _context.TemplateExtension.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
