namespace Fron.Domain.Dto.FileStorage;
public sealed record GetAllFileStorageResponseDto(
    long Id,
    string Name,
    string FileExtension,
    string StorageUrl,
    long Size,
    int FileCategoryId,
    bool Support,
    string? TemplateName,
    int? TemplateExtensionId,
    string? TemplateExtensionName,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
