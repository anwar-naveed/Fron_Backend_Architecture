using Fron.Domain.Dto.User;
using Fron.Domain.Dto.UserRegistration;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface IUserService
{
    Task<GenericResponse<UserRegistrationResponseDto>> CreateUserAsync(UserRegistrationRequestDto request);
    Task<GenericResponse<IEnumerable<GetAllUsersResponseDto>>> GetAllUsersAsync();
    Task<GenericResponse<GetUserResponseDto>> GetUserByIdAsync(long Id);
    Task<GenericResponse<UpdateUserResponseDto>> UpdateUserAsync(UpdateUserRequestDto request);
    Task<GenericResponse> DeleteUserPermAsync(long Id);
    Task<GenericResponse> DeleteUserAsync(long Id);
    Task<GenericResponse<UserRegistrationResponseDto>> AddUserRoleAsync(CreateUserRoleRequestDto request);
    Task<GenericResponse> DeleteUserRolesAsync(DeleteUserRoleRequestDto request);
}