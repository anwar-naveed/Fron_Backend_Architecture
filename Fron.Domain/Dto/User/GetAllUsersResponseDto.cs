using Fron.Domain.Dto.Role;

namespace Fron.Domain.Dto.User;
public sealed record GetAllUsersResponseDto(
    long Id,
    string? Name,
    string? Username,
    bool IsActive,
    List<GetAllRolesResponseDto>? Roles,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
