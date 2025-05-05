using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.Login;
using Fron.Domain.Dto.Role;
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
            "");
        }
        else return new UserRegistrationResponseDto(
            user.Id,
            user.Name!,
            user.Username!
        );
    }

    public static RoleRegistrationResponseDto Map(this Role role)
    {
        if (role == null)
        {
            return new RoleRegistrationResponseDto(
            0,
            "");
        }
        else return new RoleRegistrationResponseDto(
            role.Id,
            role.Name!
        );
    }
}
