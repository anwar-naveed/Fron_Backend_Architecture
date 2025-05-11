using Microsoft.AspNetCore.Http;

namespace Fron.Domain.Dto.File;
public sealed record FileUploadRequestDto(
    IFormFile? FormFile
);

