using Fron.Domain.Dto.Employee;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface IEmployeeService
{
    Task<GenericResponse> UpdateEmployeeHierarchyAsync(EmployeeHierarchyUpdateRequestDto request);
}