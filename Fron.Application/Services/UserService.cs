using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Application.Utility;
using Fron.Domain.Configuration;
using Fron.Domain.Constants;
using Fron.Domain.Dto.UserRegistration;
using Fron.Domain.GenericResponse;
using Microsoft.Extensions.Options;

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

            if (role == null)
            {
                return GenericResponse<UserRegistrationResponseDto>.Failure(ApiResponseMessages.INVALID_ROLE, ApiStatusCodes.FAILED);
            }
            else
            {
                string password = Helper.Encrypt(request.Password, _encryptionConfiguration.Key);

                return GenericResponse<UserRegistrationResponseDto>.Success(
                (await _userRepository.CreateUserAsync(request.Map(password), role)).Map(),
                ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);
            }
        }
    }
}
