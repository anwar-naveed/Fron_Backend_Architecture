using Fron.Domain.Dto.Employee;

namespace Fron.Application.Abstractions.Persistence;
public interface IEmployeeRepository
{
    Task<IEnumerable<GetAllEmployeeResponseDto>> GetAllEmployeeAsync();
    Task<GetEmployeeResponseDto?> GetByIdAsync(int businessEntityId);
}