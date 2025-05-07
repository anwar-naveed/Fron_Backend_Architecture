using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.Role;
using Fron.Domain.Dto.User;

namespace Fron.Application.Mapping;
public static class EntityToEntityMappingExtensions
{
    public static void Map(this Role entity, UpdateRoleRequestDto requestDto)
    {
        entity.Name = requestDto.Name;
        entity.IsActive = requestDto.IsActive;
        entity.ModifiedOn = DateTime.UtcNow;
    }

    public static void Map(this User entity, UpdateUserRequestDto requestDto, string password)
    {
        entity.Name = requestDto.Name;
        entity.Username = requestDto.Username;
        entity.Password = password;
        entity.IsActive = requestDto.IsActive;
        entity.ModifiedOn = DateTime.UtcNow;
    }
}
