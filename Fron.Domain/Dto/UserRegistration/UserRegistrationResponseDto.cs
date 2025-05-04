namespace Fron.Domain.Dto.UserRegistration;
public sealed record UserRegistrationResponseDto(
    long Id,
    string Name,
    string Username
);
