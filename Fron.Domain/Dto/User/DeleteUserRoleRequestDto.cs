namespace Fron.Domain.Dto.User;
public sealed record DeleteUserRoleRequestDto(
    long UserId,
    IEnumerable<long> RoleId
);
