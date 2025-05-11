namespace Fron.Domain.Dto.File;
public record FileRequestDto(
    string? url,
    string? name,
    string? extension,
    decimal size
);
