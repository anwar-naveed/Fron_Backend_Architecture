namespace Fron.Domain.Dto.User;
public sealed record UpdateUserRequestDto(
    long Id,
    string Name,
    string Username,
    string Password,
    bool IsActive
);
