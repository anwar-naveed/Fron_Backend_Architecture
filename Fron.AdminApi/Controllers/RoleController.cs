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

    [HttpPost("RoleCreate")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateRole([FromBody] RoleRegistrationRequestDto requestDto)
        => Ok(await _roleService.CreateRoleAsync(requestDto));
}
