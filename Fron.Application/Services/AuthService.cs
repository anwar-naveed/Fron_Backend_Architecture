using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Identity;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Application.Utility;
using Fron.Domain.Configuration;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Login;
using Fron.Domain.GenericResponse;
using Microsoft.Extensions.Options;

namespace Fron.Application.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly EncryptionConfiguration _encryptionConfiguration;
    private readonly ITokenService _tokenService;

    public AuthService(
        IUserRepository userRepository,
        IOptions<EncryptionConfiguration> encryptionConfiguration,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _encryptionConfiguration = encryptionConfiguration.Value;
        _tokenService = tokenService;
    }

    public async Task<GenericResponse<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto)
    {
        var encryptedPassword = Helper.Encrypt(requestDto.Password, _encryptionConfiguration.Key);

        var user = await _userRepository.GetUserAsync(requestDto.UserName, encryptedPassword);

        if (user is null)
        {
            return GenericResponse<LoginResponseDto>.Failure(ApiResponseMessages.INVALID_USERNAME_PASSWORD, ApiStatusCodes.INVALID_USERNAME_PASSWORD);
        }

        var token = _tokenService.GenerateToken(user.Username);

        var response = user.Map(token);


        return GenericResponse<LoginResponseDto>.Success(response, ApiResponseMessages.USER_LOGIN_SUCCESSFULLY, ApiStatusCodes.USER_LOGIN_SUCCESSFULLY);
    }
}
