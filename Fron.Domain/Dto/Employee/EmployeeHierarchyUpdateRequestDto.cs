namespace Fron.Domain.Dto.Employee;
public sealed record EmployeeHierarchyUpdateRequestDto(
    int BusinessEntityId,
    string? OrganizationNode,
    string LoginId,
    string JobTitle,
    DateTime HireDate,
    bool CurrentFlag
);
