using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Infrastructure;
using Fron.Application.Utility;
using Fron.Domain.Configuration;
using Fron.Domain.Constants;
using Fron.Domain.Dto.File;
using Fron.Domain.Dto.FileStorage;
using Fron.Domain.GenericResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fron.Application.Services;
public class FileService : IFileService
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IDocumentService _documentService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IFileCategoryService _fileCategoryService;
    private readonly ITemplateExtensionService _templateExtensionService;
    private readonly TemplateConfiguraion _templateConfiguration;
    private readonly NonTemplateConfiguration _nonTemplateConfiguration;

    public FileService(
        IBlobStorageService blobStorageService,
        IDocumentService documentService,
        IFileStorageService fileStorageService,
        IFileCategoryService fileCategoryService,
        ITemplateExtensionService templateExtensionService,
        IOptions<TemplateConfiguraion> templateConfiguration,
        IOptions<NonTemplateConfiguration> nonTemplateConfguration)
    {
        _blobStorageService = blobStorageService;
        _documentService = documentService;
        _fileStorageService = fileStorageService;
        _fileCategoryService = fileCategoryService;
        _templateExtensionService = templateExtensionService;
        _templateConfiguration = templateConfiguration.Value;
        _nonTemplateConfiguration = nonTemplateConfguration.Value;
    }

    public async Task<GenericResponse<FileUploadResponseDto>> FileSaveAsync(FileUploadRequestDto requestDto)
    {

        #region NullCheck
        if (requestDto == null || requestDto.FormFile == null || requestDto.FormFile.Length == 0)
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.FILE_NOT_FOUND, ApiStatusCodes.FILE_NOT_FOUND);
        }

        if (!requestDto.UploadOnBlobStorage && !requestDto.SaveInApplicationDirectory)
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.WRONG_PROPRTY_VALUE, ApiStatusCodes.FAILED);
        }

        if (!Enum.IsDefined(typeof(FileCategory), requestDto.Category))
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }

        if (requestDto.TemplateExtension != null && !Enum.IsDefined(typeof(TemplateExtension),requestDto.TemplateExtension))
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }

        if (requestDto.Category == FileCategory.Template && (requestDto.TemplateExtension == null || !Enum.IsDefined(typeof(TemplateExtension), requestDto.TemplateExtension) || string.IsNullOrEmpty(requestDto.TemplateNameWithExtension)))
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }
        #endregion

        string directoryPath;
        string filePath;
        string blobName = string.Empty;
        string strorageUrl = string.Empty;
        int fileCategoryId;
        int? templateExtensionId = null;

        var fileCategoriesResponse = await _fileCategoryService.GetAllFileCategoriesAsync();
        if (fileCategoriesResponse == null || fileCategoriesResponse.Payload!.Count() < 1)
        {
            return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
            fileCategoryId = fileCategoriesResponse.Payload!.FirstOrDefault(x => x.FileCategoryEnum == (int)requestDto.Category)!.Id;

        if (requestDto.Category == FileCategory.Template)
        {
            var templateExtensionsReponse = await _templateExtensionService.GetAllTemplateExtensionsAsync();
            if (templateExtensionsReponse == null || templateExtensionsReponse.Payload!.Count() < 1)
            {
                return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
            }
            else
                templateExtensionId = templateExtensionsReponse.Payload!.FirstOrDefault(x => x.TemplateExtensionEnum == (int)requestDto.TemplateExtension!)!.Id;
        }

        if (requestDto.Category == FileCategory.NonTemplate)
        {
            blobName = $@"{Guid.NewGuid().ToString()}_{requestDto.FormFile.FileName}";
        }
        else if (requestDto.Category == FileCategory.Template)
        {
            blobName = $@"{requestDto.FormFile.FileName}";
        }

        if (requestDto.Category == FileCategory.Template && (requestDto.TemplateExtension == TemplateExtension.Html || requestDto.TemplateExtension == TemplateExtension.HtmlResource))
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Html\");
            await _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{blobName}";
        }
        else if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension == TemplateExtension.Excel)
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Excel\");
            await _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{blobName}";
        }
        else if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension == TemplateExtension.Word)
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Word\");
            await _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{blobName}";
        }
        else
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_nonTemplateConfiguration.NonTemplateDirectory}");
            await _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}\{blobName}";
        }

        //Saving of file in application directory
        if (requestDto.SaveInApplicationDirectory)
        {
            await _documentService.CreateFileFromFormFileAsync(requestDto.FormFile, filePath);
        }

        string name = _documentService.GetFileNameWithoutExtension(blobName);
        string extension = _documentService.GetFileExtension(blobName);
        long size = requestDto.FormFile.Length;

        //Uploading file on blob storage
        if (requestDto.UploadOnBlobStorage)
        {
            strorageUrl = await _blobStorageService.UploadFileAsync(requestDto.FormFile, blobName, BlobContainerNames.DOCUMENTS_CONTAINER);
            if (string.IsNullOrEmpty(strorageUrl))
            {
                return GenericResponse<FileUploadResponseDto>.Failure(ApiResponseMessages.FILE_SAVED_FAILED, ApiStatusCodes.FILE_SAVED_FAILED);
            }
        }

        //Storing details in db
        await _fileStorageService.CreateFileStorageAsync(new FileStorageCreateRequestDto(
            blobName,
            extension,
            strorageUrl,
            size,
            fileCategoryId,
            requestDto.Support == null ? false : (bool)requestDto.Support!,
            requestDto.TemplateNameWithExtension,
            templateExtensionId
        ));

        FileUploadResponseDto responseDto = new FileUploadResponseDto(strorageUrl, name, extension, size);
        return GenericResponse<FileUploadResponseDto>.Success(responseDto, ApiResponseMessages.FILE_SAVED_SUCCESSFULLY, ApiStatusCodes.FILE_SAVED_SUCCESSFULLY);
    }

    public async Task<GenericResponse<GetFileResponseDto>> GetFileAsync(GetFileRequestDto requestDto) //modeljsonstring should be properly escaped
    {
        #region NullCheck
        if (requestDto == null)
        {
            return GenericResponse<GetFileResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }

        if (!Enum.IsDefined(typeof(FileCategory), requestDto.Category))
        {
            return GenericResponse<GetFileResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }

        if (requestDto.TemplateExtension != null && !Enum.IsDefined(typeof(TemplateExtension), requestDto.TemplateExtension))
        {
            return GenericResponse<GetFileResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }

        if (requestDto.Category == FileCategory.Template && (requestDto.TemplateExtension == null || !Enum.IsDefined(typeof(TemplateExtension), requestDto.TemplateExtension)))
        {
            return GenericResponse<GetFileResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }

        if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension != null && Enum.IsDefined(typeof(TemplateExtension), requestDto.TemplateExtension) && (string.IsNullOrEmpty(requestDto.TemplateNameWithExtension) || string.IsNullOrEmpty(requestDto.ModelJsonString)))
        {
            return GenericResponse<GetFileResponseDto>.Failure(ApiResponseMessages.INVALID_PROPERTY_FOUND, ApiStatusCodes.INVALID_PROPERTY_FOUND);
        }
        #endregion

        string directoryPath;
        string filePath;

        if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension == TemplateExtension.Html)
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Html\");
            await _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{requestDto.TemplateNameWithExtension}";
        }
        else if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension == TemplateExtension.HtmlResource)
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Html\");
            await _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{requestDto.FileNameWithExtension}";
        }
        else if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension == TemplateExtension.Excel)
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Excel\");
            await _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{requestDto.TemplateNameWithExtension}";
        }
        else if (requestDto.Category == FileCategory.Template && requestDto.TemplateExtension == TemplateExtension.Word)
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Word\");
            await _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}{requestDto.TemplateNameWithExtension}";
        }
        else
        {
            directoryPath = _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_nonTemplateConfiguration.NonTemplateDirectory}");
            await _documentService.CreateDirectoryIfNotExists(directoryPath);
            filePath = $@"{directoryPath}\{requestDto.FileNameWithExtension}";
        }

        if (requestDto.Category == FileCategory.Template) //File is template or template resource
        {
            if (!_documentService.CheckFileExists(filePath))
            {
                var fileStorage = await _fileStorageService.GetFileStorageByTemplateName(requestDto.TemplateNameWithExtension!);

                if (fileStorage == null || fileStorage.Count() < 1)
                {
                    return GenericResponse<GetFileResponseDto>.Failure(ApiResponseMessages.TEMPLATE_NOT_FOUND, ApiStatusCodes.TEMPLATE_NOT_FOUND);
                }

                foreach (var file in fileStorage)
                {
                    var blobInfo = await _blobStorageService.DownloadFileAsync(file.Name, BlobContainerNames.DOCUMENTS_CONTAINER);
                    if (blobInfo == null)
                    {
                        return GenericResponse<GetFileResponseDto>.Failure(ApiResponseMessages.TEMPLATE_NOT_FOUND, ApiStatusCodes.TEMPLATE_NOT_FOUND);
                    }

                    await _documentService.CopyToFile(blobInfo.Content, filePath); //Here checking is required
                    blobInfo.Dispose();
                }

                return await GetSuccessResponse(requestDto, filePath);
            }
            else
                return await GetSuccessResponse(requestDto, filePath);
        }
        else //File is not template
        {
            if (_documentService.CheckFileExists(filePath))
            {
                IFormFile? formFile = await _documentService.CreateFormFileFromFile(filePath, MimeTypes.OCTET)!;
                GetFileResponseDto responseDto = new GetFileResponseDto(requestDto.FileNameWithExtension!, formFile, MimeTypes.OCTET);
                return GenericResponse<GetFileResponseDto>.Success(
                    responseDto,
                    ApiResponseMessages.SUCCESS,
                    ApiStatusCodes.SUCCESS);
            }
            else
            {
                var fileStorage = await _fileStorageService.GetFileStorageByNameAsync(requestDto.FileNameWithExtension!);

                if (fileStorage == null)
                {
                    return GenericResponse<GetFileResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
                }

                var blobInfo = await _blobStorageService.DownloadFileAsync(fileStorage.StorageUrl, BlobContainerNames.DOCUMENTS_CONTAINER);

                if (blobInfo == null || blobInfo.Content.Length == 0)
                {
                    if (blobInfo != null && blobInfo.Content.Length == 0)
                    {
                        blobInfo.Dispose();
                    }

                    return GenericResponse<GetFileResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
                }
                else
                {

                    IFormFile? formFile = await _documentService.CreateFormFileFromFile(blobInfo.Content, blobInfo.Details.ContentType, requestDto.FileNameWithExtension!);
                    blobInfo.Dispose();
                    GetFileResponseDto responseDto = new GetFileResponseDto(requestDto.FileNameWithExtension!, formFile, MimeTypes.OCTET);
                    return GenericResponse<GetFileResponseDto>.Success(
                        responseDto,
                        ApiResponseMessages.SUCCESS,
                        ApiStatusCodes.SUCCESS);
                }
            }
        }
    }

    private async Task<GenericResponse<GetFileResponseDto>> GetSuccessResponse(GetFileRequestDto requestDto, string filePath)
    {
        if (requestDto.TemplateExtension == TemplateExtension.Html)
        {
            using var stream = new MemoryStream();
            var templateContent = File.ReadAllText(filePath);
            string pdfName = $"{_documentService.GetFileNameWithoutExtension(requestDto.TemplateNameWithExtension!)}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{FileExtensions.PDF}";
            dynamic? model = Helper.GetObjectFromString(requestDto.ModelJsonString);
            _documentService.GeneratePDFStream(
                 model,
                 templateContent,
                 _documentService.GetFilePathBasedExecutingAssemblyPath($@"{_templateConfiguration.TemplateDirectory}\Html\"),
                 stream);
            stream.Position = 0;
            IFormFile? formFile = await _documentService.CreateFormFileFromFile(stream, MimeTypes.PDF, pdfName);
            GetFileResponseDto responseDto = new GetFileResponseDto(pdfName, formFile, MimeTypes.PDF);
            return GenericResponse<GetFileResponseDto>.Success(
                responseDto,
                ApiResponseMessages.SUCCESS,
                ApiStatusCodes.SUCCESS);
        }
        else if (requestDto.TemplateExtension == TemplateExtension.HtmlResource)
        {
            using var stream = await _documentService.GetFileStreamAsync(filePath);
            IFormFile? formFile = await _documentService.CreateFormFileFromFile(stream, MimeTypes.OCTET, _documentService.GetFileNameFromPath(filePath));
            GetFileResponseDto responseDto = new GetFileResponseDto(formFile!.FileName, formFile, formFile.ContentType);
            return GenericResponse<GetFileResponseDto>.Success(
                responseDto,
                ApiResponseMessages.SUCCESS,
                ApiStatusCodes.SUCCESS);

        }
        else if (requestDto.TemplateExtension == TemplateExtension.Excel)
        {
            string excelName = "a"; //Todo: the working on excel template
            IFormFile? formFile = await _documentService.CreateFormFileFromFile(filePath, MimeTypes.EXCEL)!;
            GetFileResponseDto responseDto = new GetFileResponseDto(excelName, formFile, MimeTypes.EXCEL);
            return GenericResponse<GetFileResponseDto>.Success(
                responseDto,
                ApiResponseMessages.SUCCESS,
                ApiStatusCodes.SUCCESS);
        }
        else if (requestDto.TemplateExtension == TemplateExtension.Word)
        {
            string wordName = "a"; //Todo: the working on word template
            IFormFile? formFile = await _documentService.CreateFormFileFromFile(filePath, MimeTypes.WORD)!;
            GetFileResponseDto responseDto = new GetFileResponseDto(wordName, formFile, MimeTypes.WORD);
            return GenericResponse<GetFileResponseDto>.Success(
                responseDto,
                ApiResponseMessages.SUCCESS,
                ApiStatusCodes.SUCCESS);
        }
        else //Todo: define default case for template generated file
        {
            string wordName = "a";
            IFormFile? formFile = await _documentService.CreateFormFileFromFile(filePath, MimeTypes.WORD)!;
            GetFileResponseDto responseDto = new GetFileResponseDto(wordName, formFile, MimeTypes.WORD);
            return GenericResponse<GetFileResponseDto>.Success(
                responseDto,
                ApiResponseMessages.SUCCESS,
                ApiStatusCodes.SUCCESS);
        }
    }
}
