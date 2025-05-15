using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Domain.Constants;
using Fron.Domain.Dto.TemplateExtension;
using Fron.Domain.GenericResponse;
using System.Transactions;

namespace Fron.Application.Services;
public class TemplateExtensionService : ITemplateExtensionService
{
    private readonly ITemplateExtensionRepository _templateExtensionRepository;

    public TemplateExtensionService(ITemplateExtensionRepository templateExtensionRepository)
    {
        _templateExtensionRepository = templateExtensionRepository;
    }

    public async Task<GenericResponse<TemplateExtensionCreateResponseDto>> CreateTemplateExtensionAsync(TemplateExtensionCreateRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return GenericResponse<TemplateExtensionCreateResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else if (requestDto != null && !Enum.IsDefined(typeof(TemplateExtension), requestDto.TemplateExtension))
        {
            return GenericResponse<TemplateExtensionCreateResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _templateExtensionRepository.CreateTemplateExtensionAsync(requestDto!.Map());

                scope.Complete();

                return GenericResponse<TemplateExtensionCreateResponseDto>.Success(
                result.Map(),
                ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);
            }
        }
    }

    public async Task<GenericResponse<GetTemplateExtensionResponseDto>> GetTemplateExtensionByIdAsync(int Id)
    {
        var entity = await _templateExtensionRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse<GetTemplateExtensionResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<GetTemplateExtensionResponseDto>.Success(
                entity.MapGet(),
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<IEnumerable<GetAllTemplateExtensionsResponseDto>>> GetAllTemplateExtensionsAsync()
    {
        var result = await _templateExtensionRepository.GetAllTemplateExtensionsAsync();

        if (result == null || result.Count() < 1)
        {
            return GenericResponse<IEnumerable<GetAllTemplateExtensionsResponseDto>>.Failure(
                ApiResponseMessages.RECORD_NOT_FOUND,
                ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<IEnumerable<GetAllTemplateExtensionsResponseDto>>.Success(
                result,
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }
}
