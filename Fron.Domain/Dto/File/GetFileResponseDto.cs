using Microsoft.AspNetCore.Http;

namespace Fron.Domain.Dto.File;
public sealed record GetFileResponseDto(
    string FileName,
    IFormFile? FormFile,
    string mimeType
);
