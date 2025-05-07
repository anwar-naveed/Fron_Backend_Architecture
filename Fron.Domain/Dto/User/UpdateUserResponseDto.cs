namespace Fron.Domain.Dto.User;
public sealed record UpdateUserResponseDto(
    long Id,
    string Name,
    string Username,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
