using Microsoft.AspNetCore.Http;

namespace Fron.Domain.Dto.Product;
public sealed record ProductFileResponseDto(
    string FileName,
    IFormFile? FormFile,
    string mimeType
);
