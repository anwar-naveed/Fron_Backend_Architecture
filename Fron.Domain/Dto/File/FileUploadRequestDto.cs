using Microsoft.AspNetCore.Http;

namespace Fron.Domain.Dto.File;
public sealed record FileUploadRequestDto(
    Fron.Domain.Constants.FileCategory Category,
    bool UploadOnBlobStorage,
    IFormFile? FormFile,
    Fron.Domain.Constants.TemplateExtension? TemplateExtension = null,
    string? TemplateNameWithExtension = null,
    bool? Support = null
);

