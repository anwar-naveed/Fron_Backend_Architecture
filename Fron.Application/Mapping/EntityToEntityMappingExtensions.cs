using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.Role;

namespace Fron.Application.Mapping;
public static class EntityToEntityMappingExtensions
{
    public static void Map(this Role entity, UpdateRoleRequestDto requestDto)
    {
        entity.Name = requestDto.Name;
    }
}
