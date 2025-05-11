using Fron.Application.Abstractions.Application;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Role;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("Bulk-Insert-Roles")]
    public async Task<IActionResult> BulkInsertRolesAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty");
        }

        if (!Path.GetExtension(file.FileName).Equals(FileExtensions.EXCEL, StringComparison.OrdinalIgnoreCase))
            return BadRequest("File extension is not supported");

        var returnFileResponse = await _roleService.BulkInsertRolesAsync(file);

        return File(returnFileResponse.Payload!.Stream, returnFileResponse.Payload.mimeType, returnFileResponse.Payload.FileName);
    }
}
