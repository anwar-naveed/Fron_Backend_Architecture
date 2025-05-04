namespace Fron.Domain.Dto.UserRegistration;
public sealed record UserRegistrationRequestDto(
    string Name,
    string UserName,
    string Password
);