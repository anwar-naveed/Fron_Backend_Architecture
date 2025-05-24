namespace Fron.Domain.Dto.User;
public sealed record CreateUserRoleRequestDto(
    long UserId,
    long RoleId
);
