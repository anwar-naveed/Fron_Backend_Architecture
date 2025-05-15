namespace Fron.Domain.Dto.FileStorage;
public sealed record GetFileStorageResponseDto(
    long Id,
    string Name,
    string FileExtension,
    string StorageUrl,
    long Size,
    int FileCategoryId,
    bool Support,
    string? TemplateName,
    int? TemplateExtensionId,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
