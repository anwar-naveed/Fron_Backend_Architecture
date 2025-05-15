using Fron.Domain.Dto.FileCategory;
using Fron.Domain.Entities;

namespace Fron.Application.Abstractions.Persistence;
public interface IFileCategoryRepository
{
    Task<FileCategory> CreateFileCategoryAsync(FileCategory entity);
    Task DeleteFileCategoryAsync(FileCategory entity);
    Task<FileCategory?> GetByIdAsync(int id);
    Task<IEnumerable<GetAllFileCategoryResponseDto>> GetAllFileCategoriesAsync();
    Task<FileCategory> UpdateFileCategoryAsync(FileCategory entity);
}