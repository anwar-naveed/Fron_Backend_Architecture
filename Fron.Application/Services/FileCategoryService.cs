using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Domain.Constants;
using Fron.Domain.Dto.FileCategory;
using Fron.Domain.GenericResponse;
using System.Transactions;

namespace Fron.Application.Services;
public class FileCategoryService : IFileCategoryService
{
    private readonly IFileCategoryRepository _fileCategoryRepository;

    public FileCategoryService(IFileCategoryRepository fileCategoryRepository)
    {
        _fileCategoryRepository = fileCategoryRepository;
    }

    public async Task<GenericResponse<FileCategoryCreateReponseDto>> CreateFileCategoryAsync(FileCategoryCreateRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return GenericResponse<FileCategoryCreateReponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else if (requestDto != null && !Enum.IsDefined(typeof(FileCategory), requestDto.FileCategory))
        {
            return GenericResponse<FileCategoryCreateReponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _fileCategoryRepository.CreateFileCategoryAsync(requestDto!.Map());

                scope.Complete();

                return GenericResponse<FileCategoryCreateReponseDto>.Success(
                result.Map(),
                ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);
            }
        }
    }

    public async Task<GenericResponse<GetFileCategoryResponseDto>> GetFileCategoryByIdAsync(int Id)
    {
        var entity = await _fileCategoryRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse<GetFileCategoryResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<GetFileCategoryResponseDto>.Success(
                entity.MapGet(),
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<IEnumerable<GetAllFileCategoryResponseDto>>> GetAllFileCategoriesAsync()
    {
        var result = await _fileCategoryRepository.GetAllFileCategoriesAsync();

        if (result == null || result.Count() < 1)
        {
            return GenericResponse<IEnumerable<GetAllFileCategoryResponseDto>>.Failure(
                ApiResponseMessages.RECORD_NOT_FOUND,
                ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<IEnumerable<GetAllFileCategoryResponseDto>>.Success(
                result,
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }
}
