using Fron.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace Fron.Domain.Dto.File;
public sealed record FileUploadRequestDto(
    IFormFile? FormFile,
    FileCategory Category,
    TemplateExtension? TemplateExtension = null
);

