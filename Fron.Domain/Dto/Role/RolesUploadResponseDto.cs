namespace Fron.Domain.Dto.Role;
public sealed record RolesUploadResponseDto(
    string FileName,
    Stream Stream,
    string mimeType
);
