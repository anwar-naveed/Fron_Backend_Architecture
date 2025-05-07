namespace Fron.Domain.Dto.User;
public sealed record GetUserResponseDto(
    long Id,
    string? Name,
    string? Username,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
