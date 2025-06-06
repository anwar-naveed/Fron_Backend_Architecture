﻿namespace Fron.Domain.Dto.TemplateExtension;
public sealed record GetAllTemplateExtensionsResponseDto(
    int Id,
    string Name,
    int TemplateExtensionEnum,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
