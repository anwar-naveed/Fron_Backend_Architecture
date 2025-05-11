namespace Fron.Domain.Dto.File;
public sealed record FileUploadResponseDto(
    string? url,
    string? name,
    string? extension,
    long? size
);
