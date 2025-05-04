using Fron.Application.Abstractions.Application;
using Fron.Domain.Dto.UserRegistration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fron.AdminApi.Controllers;

[Route("api/user")]
public class UserController : BaseApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("UserCreate")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] UserRegistrationRequestDto requestDto)
        => Ok(await _userService.CreateUserAsync(requestDto));
}
