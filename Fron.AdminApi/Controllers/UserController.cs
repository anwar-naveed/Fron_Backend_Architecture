using Fron.Application.Abstractions.Application;
using Fron.Domain.Dto.User;
using Fron.Domain.Dto.UserRegistration;
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

    [HttpPost("User-Create")]
    public async Task<IActionResult> CreateUser([FromBody] UserRegistrationRequestDto requestDto)
        => Ok(await _userService.CreateUserAsync(requestDto));

    [HttpPut("User-Update")]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRequestDto requestDto)
        => Ok(await _userService.UpdateUserAsync(requestDto));

    [HttpDelete("User-Delete")]
    public async Task<IActionResult> DeleteUserAsync(long Id)
        => Ok(await _userService.DeleteUserAsync(Id));

    [HttpDelete("User-Delete-Perm")]
    public async Task<IActionResult> DeleteUserPermAsync(long Id)
        => Ok(await _userService.DeleteUserPermAsync(Id));

    [HttpGet("Get-All-Users")]
    public async Task<IActionResult> GetAllUsersAsync()
        => Ok(await _userService.GetAllUsersAsync());

    [HttpGet("Get-User")]
    public async Task<IActionResult> GetUserByIdAsync(long Id)
        => Ok(await _userService.GetUserByIdAsync(Id));

    [HttpPost("User-Role-Add")]
    public async Task<IActionResult> AddUserRole([FromBody] CreateUserRoleRequestDto requestDto)
        => Ok(await _userService.AddUserRoleAsync(requestDto));

    [HttpDelete("User-Delete-Roles")]
    public async Task<IActionResult> DeleteUserRolesAsync([FromBody] DeleteUserRoleRequestDto requestDto)
        => Ok(await _userService.DeleteUserRolesAsync(requestDto));
}
