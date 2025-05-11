using Fron.Domain.Dto.File;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface IFileService
{
    Task<GenericResponse<FileUploadResponseDto>> FileSaveAsync(FileUploadRequestDto requestDto);
}
