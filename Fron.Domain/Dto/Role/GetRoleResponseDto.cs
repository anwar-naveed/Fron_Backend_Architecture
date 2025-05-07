namespace Fron.Domain.Dto.Role;
public sealed record GetRoleResponseDto(
    long Id,
    string Name,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
