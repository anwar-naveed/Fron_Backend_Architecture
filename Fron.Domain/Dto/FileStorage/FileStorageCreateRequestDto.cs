namespace Fron.Domain.Dto.FileStorage;
public sealed record FileStorageCreateRequestDto(
    string Name,
    string FileExtension,
    string StorageUrl,
    long Size,
    int FileCategoryId,
    bool Support,
    string? TemplateName,
    int? TemplateExtensionId
);
