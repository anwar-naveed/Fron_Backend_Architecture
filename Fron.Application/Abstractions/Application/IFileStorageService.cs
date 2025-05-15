using Fron.Domain.Dto.FileStorage;
using Fron.Domain.Entities;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface IFileStorageService
{
    Task<GenericResponse<FileStorageCreateResponseDto>> CreateFileStorageAsync(FileStorageCreateRequestDto requestDto);
    Task<GenericResponse<GetFileStorageResponseDto>> GetFileStorageByIdAsync(long Id);
    Task<FileStorage?> GetFileStorageByNameAsync(string fileName);
    Task<IEnumerable<FileStorage>> GetFileStorageByTemplateName(string templateName);
    Task<IEnumerable<FileStorage>> GetFileStorageByTemplateName(string templateName, bool support);
    Task<GenericResponse<IEnumerable<GetAllFileStorageResponseDto>>> GetAllFileStoragesAsync();
}