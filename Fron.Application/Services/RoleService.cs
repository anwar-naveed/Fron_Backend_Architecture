using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Role;
using Fron.Domain.GenericResponse;

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
            return GenericResponse<RoleRegistrationResponseDto>.Success(
                (await _roleRepository.CreateRoleAsync(request.Map())).Map(),
                ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);

        }
    }
}
