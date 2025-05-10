using Fron.Application.Abstractions.Application;
using Fron.Domain.Dto.Employee;
using Microsoft.AspNetCore.Mvc;

namespace Fron.AdminApi.Controllers;
[Route("api/employee")]
public class EmployeeController : BaseApiController
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPut("Employee-Hierarchy-Update")]
    public async Task<IActionResult> UpdateEmployeeHierarchyAsync([FromBody] EmployeeHierarchyUpdateRequestDto requestDto)
        => Ok(await _employeeService.UpdateEmployeeHierarchyAsync(requestDto));
}
