using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Infrastructure;
using Fron.Domain.Configuration;
using Fron.Domain.Constants;
using Fron.Domain.Dto.File;
using Fron.Domain.GenericResponse;
using Microsoft.Extensions.Options;

namespace Fron.Application.Services;
public class FileService : IFileService
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IDocumentService _documentService;
    private readonly TemplateConfiguraion _templateConfiguration;
    private readonly NonTemplateConfiguration _nonTemplateConfiguration;

    public FileService(
        IBlobStorageService blobStorageService,
        IDocumentService documentService,
        IOptions<TemplateConfiguraion> templateConfiguration,
        IOptions<NonTemplateConfiguration> nonTemplateConfguration)
    {
        _blobStorageService = blobStorageService;
        _documentService = documentService;
        _templateConfiguration = templateConfiguration.Value;
        _nonTemplateConfiguration = nonTemplateConfguration.Value;
    }

    public async Task<GenericResponse<FileUploadResponseDto>> FileSaveAsync(FileUploadRequestDto requestDto)
    {

        if (requestDto == null || requestDto.FormFile == null || requestDto.FormFile.Length == 0)
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.FILE_NOT_FOUND, ApiStatusCodes.FILE_NOT_FOUND);
        }

        if (!Enum.TryParse<FileCategory>(requestDto.Category.ToString(), out _))
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }

        if (requestDto.TemplateExtension != null && !Enum.TryParse<TemplateExtension>(requestDto.TemplateExtension.ToString(), out _))
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }

        if (requestDto.Category == FileCategory.Template && (requestDto.TemplateExtension == null || !Enum.TryParse<TemplateExtension>(requestDto.TemplateExtension.ToString(), out _)))
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }

        string directoryPath;
        string filePath;
        string blobName = string.Empty;

        if (requestDto.Category == FileCategory.NonTemplate)
        {
            blobName = $@"{Guid.NewGuid().ToString()}_{requestDto.FormFile.FileName}";
        }
        else if (requestDto.Category == FileCategory.Template)
        {
            blobName = $@"{requestDto.FormFile.FileName}";
        }

        if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension == TemplateExtension.Html)
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Html\");
            _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{blobName}";
        }
        else if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension == TemplateExtension.Excel)
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Excel\");
            _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{blobName}";
        }
        else if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension == TemplateExtension.Word)
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Word\");
            _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{blobName}";
        }
        else
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_nonTemplateConfiguration.NonTemplateDirectory}");
            _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}\{blobName}";
        }

        //Saving of file in application directory
        await _documentService.CreateFileFromFormFileAsync(requestDto.FormFile, filePath);

        string name = _documentService.GetFileNameWithoutExtension(blobName);
        string extension = _documentService.GetFileExtension(requestDto.FormFile.FileName);
        long size = requestDto.FormFile.Length;

        var strorageUrl = filePath;
        //var strorageUrl = await _blobStorageService.UploadFileAsync(requestDto.FormFile, blobName, BlobContainerNames.DOCUMENTS_CONTAINER);
        //if (string.IsNullOrEmpty(strorageUrl))
        //{
        //    return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.FILE_SAVED_FAILED, ApiStatusCodes.FILE_SAVED_FAILED);
        //}
        
        //Todo: storage of details in db


        FileUploadResponseDto responseDto = new FileUploadResponseDto(strorageUrl, name, extension, size);
        return GenericResponse<FileUploadResponseDto>.Success(responseDto, ApiResponseMessages.FILE_SAVED_SUCCESSFULLY, ApiStatusCodes.FILE_SAVED_SUCCESSFULLY);
    }
}
