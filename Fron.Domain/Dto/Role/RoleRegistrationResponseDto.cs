namespace Fron.Domain.Dto.Role;
public sealed record RoleRegistrationResponseDto(
    long Id,
    string Name,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
