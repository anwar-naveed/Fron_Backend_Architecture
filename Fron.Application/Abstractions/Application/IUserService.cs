using Fron.Domain.Dto.UserRegistration;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface IUserService
{
    Task<GenericResponse<UserRegistrationResponseDto>> CreateUserAsync(UserRegistrationRequestDto request);
}