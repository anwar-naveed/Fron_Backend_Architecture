namespace Fron.Domain.Dto.FileCategory;
public sealed record GetFileCategoryResponseDto(
    int Id,
    string Name,
    int FileCategoryEnum,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
