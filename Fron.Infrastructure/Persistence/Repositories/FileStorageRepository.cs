using Fron.Application.Abstractions.Persistence;
using Fron.Domain.Dto.FileStorage;
using Fron.Domain.Entities;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Repositories;
public class FileStorageRepository : BaseRepository, IFileStorageRepository
{
    public FileStorageRepository(DataDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<FileStorage> CreateFileStorageAsync(FileStorage entity)
    {
        var fileStorage = await _context.FileStorage.AddAsync(entity);
        await _context.SaveChangesAsync();
        return fileStorage.Entity;
    }

    public async Task<FileStorage> UpdateFileStorageAsync(FileStorage entity)
    {
        var fileStorage = _context.FileStorage.Update(entity);
        await _context.SaveChangesAsync();
        return fileStorage.Entity;
    }

    public async Task<FileStorage?> GetByIdAsync(long id)
    {
        return await _context.FileStorage
            .Where(e => e.Id == id && e.IsActive == true)
            .FirstOrDefaultAsync();
    }

    public async Task<FileStorage?> GetByNameAsync(string fileName)
    {
        return await _context.FileStorage
            .Where(e => e.Name == fileName && e.IsActive == true)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<GetAllFileStorageResponseDto>> GetAllFileStorageAsync()
    {
        return await _context.FileStorage
            .Where(x => x.IsActive == true)
            .Select(x => new GetAllFileStorageResponseDto(
                x.Id,
                x.Name,
                x.FileExtension,
                x.StorageUrl,
                x.Size,
                x.FileCategoryId,
                x.Support,
                x.TemplateName,
                x.TemplateExtensionId,
                x.TemplateExtension != null ? x.TemplateExtension.Name : "",
                x.IsActive,
                x.CreatedOn,
                x.ModifiedOn))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<FileStorage>> GetByTemplateName(string templateName)
    {
        return await _context.FileStorage
            .Where(e => e.TemplateName == templateName && e.IsActive == true)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<FileStorage>> GetByTemplateName(string templateName, bool support)
    {
        return await _context.FileStorage
            .Where(e => e.TemplateName == templateName && e.IsActive == true && e.Support == support)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task DeleteFileStorageAsync(FileStorage entity)
    {
        _context.FileStorage.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
