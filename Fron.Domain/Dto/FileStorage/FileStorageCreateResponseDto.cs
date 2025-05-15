namespace Fron.Domain.Dto.FileStorage;
public sealed record FileStorageCreateResponseDto(
    long Id,
    string Name,
    string FileExtension,
    string StorageUrl,
    long Size,
    int FileCategoryId,
    string FileCategoryName,
    bool Support,
    string? TemplateName,
    int? TemplateExtensionId,
    string? TemplateExtensionName,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);