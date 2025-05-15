namespace Fron.Domain.Dto.FileCategory;
public sealed record FileCategoryCreateReponseDto(
    int Id,
    string Name,
    int FileCategoryEnum,
    bool IsActive,
    DateTime CreatedOn,
    DateTime ModifiedOn
);
