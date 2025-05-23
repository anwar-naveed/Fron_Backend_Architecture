﻿namespace Fron.Domain.Dto.Role;
public sealed record GetAllRolesResponseDto(
    long Id,
    string? Name,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
