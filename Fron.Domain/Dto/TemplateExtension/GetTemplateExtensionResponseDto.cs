namespace Fron.Domain.Dto.TemplateExtension;
public sealed record GetTemplateExtensionResponseDto(
    int Id,
    string Name,
    int TemplateExtensionEnum,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
