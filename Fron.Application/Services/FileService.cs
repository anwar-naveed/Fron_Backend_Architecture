using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Infrastructure;
using Fron.Domain.Constants;
using Fron.Domain.Dto.File;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Services;
public class FileService : IFileService
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IDocumentService _documentService;

    public FileService(
        IBlobStorageService blobStorageService,
        IDocumentService documentService)
    {
        _blobStorageService = blobStorageService;
        _documentService = documentService;
    }

    public async Task<GenericResponse<FileUploadResponseDto>> FileSaveAsync(FileUploadRequestDto requestDto)
    {

        if (requestDto == null || requestDto.FormFile == null || requestDto.FormFile.Length == 0)
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.FILE_NOT_FOUND, ApiStatusCodes.FILE_NOT_FOUND);
        }

        var strorageUrl = await _blobStorageService.UploadFileAsync(requestDto.FormFile, BlobContainerNames.DOCUMENTS_CONTAINER);
        if (string.IsNullOrEmpty(strorageUrl))
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.FILE_SAVED_FAILED, ApiStatusCodes.FILE_SAVED_FAILED);
        }
        string name = _documentService.GetFileNameWithoutExtension(requestDto.FormFile.FileName);
        string extension = _documentService.GetFileExtension(requestDto.FormFile.FileName);
        long size = requestDto.FormFile.Length;
        FileUploadResponseDto responseDto = new FileUploadResponseDto(strorageUrl, name, extension, size);
        return GenericResponse<FileUploadResponseDto>.Success(responseDto, ApiResponseMessages.FILE_SAVED_SUCCESSFULLY, ApiStatusCodes.FILE_SAVED_SUCCESSFULLY);


    }
}
