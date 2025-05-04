using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Domain.Constants;
using Fron.Domain.Dto.UserRegistration;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GenericResponse<UserRegistrationResponseDto>> CreateUserAsync(UserRegistrationRequestDto request)
    {
        if (request == null)
        {
            return GenericResponse<UserRegistrationResponseDto>.Failure(ApiResponseMessages.INVALID_USERNAME_PASSWORD, ApiStatusCodes.FAILED);
        }
        else
        {
            return GenericResponse<UserRegistrationResponseDto>.Success(
                (await _userRepository.CreateUserAsync(request.Map())).Map(),
                ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);

        }
    }
}
