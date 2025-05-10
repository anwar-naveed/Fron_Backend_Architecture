using Fron.Application.Abstractions.Persistence;
using Fron.Domain.Dto.Employee;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Repositories;
public class EmployeeRepository : BaseRepository, IEmployeeRepository
{
    public EmployeeRepository(DataDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<GetAllEmployeeResponseDto>> GetAllEmployeeAsync()
    {
        return await _context.Employee
            .Select(x => new GetAllEmployeeResponseDto(
                x.BusinessEntityId,
                x.NationalIdnumber,
                x.LoginId,
                x.OrganizationNode,
                x.OrganizationLevel,
                x.JobTitle,
                x.BirthDate,
                x.MaritalStatus,
                x.Gender,
                x.HireDate,
                x.SalariedFlag,
                x.VacationHours,
                x.SickLeaveHours,
                x.CurrentFlag,
                x.Rowguid,
                x.ModifiedDate
                ))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<GetEmployeeResponseDto?> GetByIdAsync(int businessEntityId)
    {
        return await _context.Employee
            .Where(x => x.BusinessEntityId == businessEntityId)
            .Select(x => new GetEmployeeResponseDto(
                x.BusinessEntityId,
                x.NationalIdnumber,
                x.LoginId,
                x.OrganizationNode,
                x.OrganizationLevel,
                x.JobTitle,
                x.BirthDate,
                x.MaritalStatus,
                x.Gender,
                x.HireDate,
                x.SalariedFlag,
                x.VacationHours,
                x.SickLeaveHours,
                x.CurrentFlag,
                x.Rowguid,
                x.ModifiedDate
                ))
            .FirstOrDefaultAsync();
    }
}
