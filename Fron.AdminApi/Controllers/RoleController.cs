using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Infrastructure;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Role;
using Microsoft.AspNetCore.Mvc;

namespace Fron.AdminApi.Controllers;

[Route("api/role")]
public class RoleController : BaseApiController
{
    private readonly IRoleService _roleService;
    private readonly IDocumentService _documentService;

    public RoleController(
        IRoleService roleService,
        IDocumentService documentService)
    {
        _roleService = roleService;
        _documentService = documentService;
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

        if (!_documentService.GetFileExtension(file.FileName).Equals(FileExtensions.EXCEL, StringComparison.OrdinalIgnoreCase))
            return BadRequest("File extension is not supported");

        return Ok(await _roleService.BulkInsertRolesAsync(file));
    }
}
