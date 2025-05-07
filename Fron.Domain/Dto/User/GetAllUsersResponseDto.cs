namespace Fron.Domain.Dto.User;
public sealed record GetAllUsersResponseDto(
    long Id,
    string? Name,
    string? Username,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
