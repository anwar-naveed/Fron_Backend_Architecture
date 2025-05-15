using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Domain.Constants;
using Fron.Domain.Dto.FileStorage;
using Fron.Domain.Entities;
using Fron.Domain.GenericResponse;
using System.Transactions;

namespace Fron.Application.Services;
public class FileStorageService : IFileStorageService
{
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly IFileCategoryRepository _fileCategoryRepository;
    private readonly ITemplateExtensionRepository _templateExtensionRepository;

    public FileStorageService(
        IFileStorageRepository fileStorageRepository,
        IFileCategoryRepository fileCategoryRepository,
        ITemplateExtensionRepository templateExtensionRepository)
    {
        _fileStorageRepository = fileStorageRepository;
        _fileCategoryRepository = fileCategoryRepository;
        _templateExtensionRepository = templateExtensionRepository;
    }

    public async Task<GenericResponse<FileStorageCreateResponseDto>> CreateFileStorageAsync(FileStorageCreateRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return GenericResponse<FileStorageCreateResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var fileCategory = await _fileCategoryRepository.GetByIdAsync(requestDto.FileCategoryId);

                if (fileCategory == null)
                {
                    return GenericResponse<FileStorageCreateResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
                }

                if (requestDto.TemplateExtensionId != null)
                {
                    var templateExtension = await _templateExtensionRepository.GetByIdAsync(requestDto.TemplateExtensionId!);

                    if (templateExtension == null)
                    {
                        return GenericResponse<FileStorageCreateResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
                    }
                }

                var result = await _fileStorageRepository.CreateFileStorageAsync(requestDto!.Map());

                scope.Complete();

                return GenericResponse<FileStorageCreateResponseDto>.Success(
                result.Map(),
                ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);
            }
        }
    }

    public async Task<GenericResponse<GetFileStorageResponseDto>> GetFileStorageByIdAsync(long Id)
    {
        var entity = await _fileStorageRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse<GetFileStorageResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<GetFileStorageResponseDto>.Success(
                entity.MapGet(),
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }

    public async Task<FileStorage?> GetFileStorageByNameAsync(string fileName)
    {
        return await _fileStorageRepository.GetByNameAsync(fileName);
    }

    public async Task<IEnumerable<FileStorage>> GetFileStorageByTemplateName(string templateName)
    {
        return await _fileStorageRepository.GetByTemplateName(templateName);
    }

    public async Task<IEnumerable<FileStorage>> GetFileStorageByTemplateName(string templateName, bool support)
    {
        return await _fileStorageRepository.GetByTemplateName(templateName, support);
    }

    public async Task<GenericResponse<IEnumerable<GetAllFileStorageResponseDto>>> GetAllFileStoragesAsync()
    {
        var result = await _fileStorageRepository.GetAllFileStorageAsync();

        if (result == null || result.Count() < 1)
        {
            return GenericResponse<IEnumerable<GetAllFileStorageResponseDto>>.Failure(
                ApiResponseMessages.RECORD_NOT_FOUND,
                ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<IEnumerable<GetAllFileStorageResponseDto>>.Success(
                result,
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }
}
