using Fron.Domain.Dto.Login;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface IAuthService
{
    Task<GenericResponse<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto);
}