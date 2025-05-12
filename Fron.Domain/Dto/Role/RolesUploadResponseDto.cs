using Microsoft.AspNetCore.Http;

namespace Fron.Domain.Dto.Role;
public sealed record RolesUploadResponseDto(
    string FileName,
    IFormFile? FormFile,
    string mimeType
);
