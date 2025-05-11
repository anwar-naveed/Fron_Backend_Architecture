namespace Fron.Domain.Dto.Product;
public sealed record ProductFileResponseDto(
    string FileName,
    Stream Stream,
    string mimeType
);
