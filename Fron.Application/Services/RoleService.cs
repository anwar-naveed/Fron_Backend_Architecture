using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Role;
using Fron.Domain.GenericResponse;
using System.Transactions;

namespace Fron.Application.Services;
public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<GenericResponse<RoleRegistrationResponseDto>> CreateRoleAsync(RoleRegistrationRequestDto request)
    {
        if (request == null)
        {
            return GenericResponse<RoleRegistrationResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _roleRepository.CreateRoleAsync(request.Map());

                scope.Complete();

                return GenericResponse<RoleRegistrationResponseDto>.Success(
                result.Map(),
                ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);
            }
        }
    }

    public async Task<GenericResponse<IEnumerable<GetAllRolesResponseDto>>> GetAllRolesAsync()
    {
        var result = await _roleRepository.GetAllRolesAsync();

        if (result == null || result.Count() < 0)
        {
            return GenericResponse<IEnumerable<GetAllRolesResponseDto>>.Failure(
                ApiResponseMessages.RECORD_NOT_FOUND,
                ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<IEnumerable<GetAllRolesResponseDto>>.Success(
                result,
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<GetRoleResponseDto>> GetRoleByIdAsync(long Id)
    {
        var entity = await _roleRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse<GetRoleResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<GetRoleResponseDto>.Success(
                entity.MapGet(),
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<UpdateRoleResponseDto>> UpdateRoleAsync(UpdateRoleRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return GenericResponse<UpdateRoleResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = await _roleRepository.GetByIdAsync(requestDto.Id);

                if (entity == null)
                {
                    return GenericResponse<UpdateRoleResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.FAILED);
                }
                else
                {
                    entity.Map(requestDto);

                    var updatedEntity = await _roleRepository.UpdateRoleAsync(entity);

                    scope.Complete();

                    return GenericResponse<UpdateRoleResponseDto>.Success(
                        updatedEntity.MapUpdate(),
                        ApiResponseMessages.RECORD_UPDATED_SUCCESSFULLY,
                        ApiStatusCodes.RECORD_UPDATED_SUCCESSFULLY);
                }
            }
        }
    }

    public async Task<GenericResponse> DeleteRoleAsync(long Id)
    {
        var entity = await _roleRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            await _roleRepository.DeleteRoleAsync(entity);
            return GenericResponse.Success(ApiResponseMessages.RECORD_DELETED_SUCCESSFULLY, ApiStatusCodes.RECORD_DELETED_SUCCESSFULLY);
        }
    }
}
