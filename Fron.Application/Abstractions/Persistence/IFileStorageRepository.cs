using Fron.Domain.Dto.FileStorage;
using Fron.Domain.Entities;

namespace Fron.Application.Abstractions.Persistence;
public interface IFileStorageRepository
{
    Task<FileStorage> CreateFileStorageAsync(FileStorage entity);
    Task<FileStorage> UpdateFileStorageAsync(FileStorage entity);
    Task<FileStorage?> GetByIdAsync(long id);
    Task<FileStorage?> GetByNameAsync(string fileName);
    Task<IEnumerable<GetAllFileStorageResponseDto>> GetAllFileStorageAsync();
    Task<IEnumerable<FileStorage>> GetByTemplateName(string templateName);
    Task<IEnumerable<FileStorage>> GetByTemplateName(string templateName, bool support);
    Task DeleteFileStorageAsync(FileStorage entity);

}