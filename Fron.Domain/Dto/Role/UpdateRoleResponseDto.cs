﻿namespace Fron.Domain.Dto.Role;
public sealed record UpdateRoleResponseDto(
    long Id,
    string Name,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
