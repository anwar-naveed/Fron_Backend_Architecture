using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.Login;
using Fron.Domain.Dto.Role;
using Fron.Domain.Dto.User;
using Fron.Domain.Dto.UserRegistration;

namespace Fron.Application.Mapping;
public static class EntityToDtoMappingExtensions
{
    public static LoginResponseDto Map(this User a, string token)
    {
        if (a == null && !string.IsNullOrEmpty(token))
        {
            return new LoginResponseDto(
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            [],
            "",
            "",
            "");
        }
        else return new LoginResponseDto(
            "",
            "",
            "",
            "",
            a!.Username,
            "",
            "",
            "",
            [],
            "",
            "",
            token
        );
    }

    public static UserRegistrationResponseDto Map(this User user)
    {
        if (user == null)
        {
            return new UserRegistrationResponseDto(
            0,
            "",
            "",
            true,
            DateTime.UtcNow,
            DateTime.UtcNow);
        }
        else return new UserRegistrationResponseDto(
            user.Id,
            user.Name!,
            user.Username!,
            user.IsActive,
            user.CreatedOn,
            user.ModifiedOn
        );
    }

    public static RoleRegistrationResponseDto Map(this Role role)
    {
        if (role == null)
        {
            return new RoleRegistrationResponseDto(
            0,
            "",
            true,
            DateTime.UtcNow,
            DateTime.UtcNow);
        }
        else return new RoleRegistrationResponseDto(
            role.Id,
            role.Name!,
            role.IsActive,
            role.CreatedOn,
            role.ModifiedOn
        );
    }

    public static UpdateRoleResponseDto MapUpdate(this Role role)
    {
        if (role == null)
        {
            return new UpdateRoleResponseDto(
                0,
                "",
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new UpdateRoleResponseDto(
            role.Id,
            role.Name!,
            role.IsActive,
            role.CreatedOn,
            role.ModifiedOn);
    }

    public static GetRoleResponseDto MapGet(this Role role)
    {
        if (role == null)
        {
            return new GetRoleResponseDto(
                0,
                "",
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new GetRoleResponseDto(
            role.Id,
            role.Name!,
            role.IsActive,
            role.CreatedOn,
            role.ModifiedOn);
    }

    public static GetUserResponseDto MapGet(this User user)
    {
        if (user == null)
        {
            return new GetUserResponseDto(
                0,
                "",
                "",
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new GetUserResponseDto(
            user.Id,
            user.Name!,
            user.Username!,
            user.IsActive,
            user.CreatedOn,
            user.ModifiedOn);
    }

    public static UpdateUserResponseDto MapUpdate(this User user)
    {
        if (user == null)
        {
            return new UpdateUserResponseDto(
                0,
                "",
                "",
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new UpdateUserResponseDto(
            user.Id,
            user.Name!,
            user.Username!,
            user.IsActive,
            user.CreatedOn,
            user.ModifiedOn);
    }
}
