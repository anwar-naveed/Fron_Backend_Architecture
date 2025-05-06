using Microsoft.AspNetCore.Mvc;
using Fron.Application.Abstractions.Application;
using Microsoft.AspNetCore.Authorization;
using Fron.Domain.Dto.Role;

namespace Fron.AdminApi.Controllers;

[Route("api/role")]
public class RoleController : BaseApiController
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("Role-Create")]
    public async Task<IActionResult> CreateRoleAsync([FromBody] RoleRegistrationRequestDto requestDto)
        => Ok(await _roleService.CreateRoleAsync(requestDto));

    [HttpPut("Role-Update")]
    public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequestDto requestDto)
        => Ok(await _roleService.UpdateRoleAsync(requestDto));

    [HttpDelete("Role-Delete")]
    public async Task<IActionResult> DeleteRoleAsync(long Id)
        => Ok(await _roleService.DeleteRoleAsync(Id));

    [HttpGet("Get-All-Roles")]
    public async Task<IActionResult> GetAllRolesAsync()
        => Ok(await _roleService.GetAllRolesAsync());

    [HttpGet("Get-Role")]
    public async Task<IActionResult> GetRoleByIdAsync(long Id)
        => Ok(await _roleService.GetRoleByIdAsync(Id));
}
