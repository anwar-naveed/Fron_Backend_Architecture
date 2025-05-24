using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Application.Utility;
using Fron.Domain.Configuration;
using Fron.Domain.Constants;
using Fron.Domain.Dto.User;
using Fron.Domain.Dto.UserRegistration;
using Fron.Domain.GenericResponse;
using Microsoft.Extensions.Options;
using System.Transactions;

namespace Fron.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly EncryptionConfiguration _encryptionConfiguration;

    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IOptions<EncryptionConfiguration> encryptionConfiguration)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _encryptionConfiguration = encryptionConfiguration.Value;
    }

    public async Task<GenericResponse<UserRegistrationResponseDto>> CreateUserAsync(UserRegistrationRequestDto request)
    {
        if (request == null)
        {
            return GenericResponse<UserRegistrationResponseDto>.Failure(ApiResponseMessages.INVALID_USERNAME_PASSWORD, ApiStatusCodes.FAILED);
        }
        else
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);

            if(role == null)
            {
                return GenericResponse<UserRegistrationResponseDto>.Failure(ApiResponseMessages.INVALID_ROLE, ApiStatusCodes.FAILED);
            }
            else
            {
                return GenericResponse<UserRegistrationResponseDto>.Success(
                (await _userRepository.CreateUserAsync(request.Map(_encryptionConfiguration.Key), role)).Map(),
                ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);
            }
        }
    }

    public async Task<GenericResponse<IEnumerable<GetAllUsersResponseDto>>> GetAllUsersAsync()
    {
        var result = await _userRepository.GetAllUsersAsync();

        if (result == null || result.Count() < 1)
        {
            return GenericResponse<IEnumerable<GetAllUsersResponseDto>>.Failure(
                ApiResponseMessages.RECORD_NOT_FOUND,
                ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<IEnumerable<GetAllUsersResponseDto>>.Success(
                result,
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<GetUserResponseDto>> GetUserByIdAsync(long Id)
    {
        var entity = await _userRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse<GetUserResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<GetUserResponseDto>.Success(
                entity.MapGet(),
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<UpdateUserResponseDto>> UpdateUserAsync(UpdateUserRequestDto request)
    {
        if (request == null)
        {
            return GenericResponse<UpdateUserResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = await _userRepository.GetByIdAsync(request.Id);

                if (entity == null)
                {
                    return GenericResponse<UpdateUserResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
                }
                else
                {
                    entity.Map(request, _encryptionConfiguration.Key);

                    var updatedEntity = await _userRepository.UpdateUserAsync(entity);

                    scope.Complete();

                    return GenericResponse<UpdateUserResponseDto>.Success(
                        updatedEntity.MapUpdate(),
                        ApiResponseMessages.RECORD_UPDATED_SUCCESSFULLY,
                        ApiStatusCodes.RECORD_UPDATED_SUCCESSFULLY);
                } 
            }
        }
    }

    public async Task<GenericResponse> DeleteUserPermAsync(long Id)
    {
        var entity = await _userRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            await _userRepository.DeleteUserAsync(entity);
            return GenericResponse.Success(ApiResponseMessages.RECORD_DELETED_SUCCESSFULLY, ApiStatusCodes.RECORD_DELETED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse> DeleteUserAsync(long Id)
    {
        var entity = await _userRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            entity.IsActive = false;
            await _userRepository.UpdateUserAsync(entity);
            return GenericResponse.Success(ApiResponseMessages.RECORD_DELETED_SUCCESSFULLY, ApiStatusCodes.RECORD_DELETED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<UserRegistrationResponseDto>> AddUserRoleAsync(CreateUserRoleRequestDto request)
    {
        if (request == null)
        {
            return GenericResponse<UserRegistrationResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId);

            if (role == null)
            {
                return GenericResponse<UserRegistrationResponseDto>.Failure(ApiResponseMessages.INVALID_ROLE, ApiStatusCodes.FAILED);
            }
            else
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);

                if (user == null)
                {
                    return GenericResponse<UserRegistrationResponseDto>.Failure(ApiResponseMessages.INVALID_USER, ApiStatusCodes.FAILED);
                }

                if (user.UserRoles != null && user.UserRoles.Count > 0 && user.UserRoles.Any(x => x.RoleId == role.Id))
                {
                    return GenericResponse<UserRegistrationResponseDto>.Failure(ApiResponseMessages.RECORD_ALREADY_EXIST, ApiStatusCodes.RECORD_ALREADY_EXIST);
                }
                else
                {


                    return GenericResponse<UserRegistrationResponseDto>.Success(
                    (await _userRepository.AddUserRoleAsync(user, role)).Map(),
                    ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                    ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);
                }
            }
        }
    }
}
