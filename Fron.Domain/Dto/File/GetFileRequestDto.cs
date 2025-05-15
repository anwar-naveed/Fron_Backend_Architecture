namespace Fron.Domain.Dto.File;
public sealed record GetFileRequestDto(
    Fron.Domain.Constants.FileCategory Category,
    string FileNameWithExtension,
    string? ModelJsonString,
    Fron.Domain.Constants.TemplateExtension? TemplateExtension = null,
    string? TemplateNameWithExtension = null
);
