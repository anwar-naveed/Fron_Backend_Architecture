using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.Login;
using Fron.Domain.Dto.Role;
using Fron.Domain.Dto.UserRegistration;

namespace Fron.Application.Mapping;
public static class DtoToEntityMappingExtensions
{
    public static User Map(this LoginRequestDto requestDto)
    {
        if (requestDto == null) { return new User(); }
        else return new User
        {
            Username = requestDto.UserName,
            Password = requestDto.Password
        };
    }

    public static User Map(this UserRegistrationRequestDto requestDto)
    {
        if (requestDto == null) { return new User(); }
        else return new User
        {
            Name = requestDto.Name,
            Username = requestDto.UserName,
            Password = requestDto.Password
        };
    }

    public static Role Map(this RoleRegistrationRequestDto requestDto)
    {
        if (requestDto == null) { return new Role(); }
        else return new Role
        {
            Name = requestDto.Name
        };
    }
}
