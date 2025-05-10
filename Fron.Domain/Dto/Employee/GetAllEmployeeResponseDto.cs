using Microsoft.EntityFrameworkCore;

namespace Fron.Domain.Dto.Employee;
public sealed record GetAllEmployeeResponseDto(
    int BusinessEnityId,
    string NationalIdnumber,
    string LoginId,
    HierarchyId? OrganizationNode,
    short? OrganizationLevel,
    string JobTitle,
    DateOnly BirthDate,
    string MaritalStatus,
    string Gender,
    DateOnly HireDate,
    bool SalariedFlag,
    short VacationHours,
    short SickLeaveHours,
    bool CurrentFlag,
    Guid Rowguid,
    DateTime ModifiedDate
);
