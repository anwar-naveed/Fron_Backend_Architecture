using Fron.Application.Utility;
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

    public static void Map(this User entity, UpdateUserRequestDto requestDto, string key)
    {
        entity.Name = Helper.Base64Decode(requestDto.Name);
        entity.Username = Helper.Base64Decode(requestDto.Username);
        entity.Password = Helper.Encrypt(Helper.Base64Decode(requestDto.Password), key);
        //entity.IsActive = requestDto.IsActive;
        entity.ModifiedOn = DateTime.UtcNow;
    }
}
