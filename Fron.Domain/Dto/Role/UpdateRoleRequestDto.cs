namespace Fron.Domain.Dto.Role;
public sealed record UpdateRoleRequestDto(
    long Id,
    string Name,
    bool IsActive
);
