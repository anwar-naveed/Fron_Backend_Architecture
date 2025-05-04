using Fron.Domain.Dto.Role;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface IRoleService
{
    Task<GenericResponse<RoleRegistrationResponseDto>> CreateRoleAsync(RoleRegistrationRequestDto request);
}