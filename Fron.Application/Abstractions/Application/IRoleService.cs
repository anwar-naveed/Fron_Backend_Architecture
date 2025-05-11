using Fron.Domain.Dto.Role;
using Fron.Domain.GenericResponse;
using Microsoft.AspNetCore.Http;

namespace Fron.Application.Abstractions.Application;
public interface IRoleService
{
    Task<GenericResponse<RoleRegistrationResponseDto>> CreateRoleAsync(RoleRegistrationRequestDto request);
    Task<GenericResponse<IEnumerable<GetAllRolesResponseDto>>> GetAllRolesAsync();
    Task<GenericResponse<GetRoleResponseDto>> GetRoleByIdAsync(long Id);
    Task<GenericResponse<UpdateRoleResponseDto>> UpdateRoleAsync(UpdateRoleRequestDto requestDto);
    Task<GenericResponse> DeleteRoleAsync(long Id);
    Task<GenericResponse<RolesUploadResponseDto>> BulkInsertRolesAsync(IFormFile file);
}