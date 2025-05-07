namespace Fron.Domain.Dto.UserRegistration;
public sealed record UserRegistrationResponseDto(
    long Id,
    string Name,
    string Username,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
