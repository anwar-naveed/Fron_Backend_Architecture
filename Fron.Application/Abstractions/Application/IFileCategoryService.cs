using Fron.Domain.Dto.FileCategory;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface IFileCategoryService
{
    Task<GenericResponse<FileCategoryCreateReponseDto>> CreateFileCategoryAsync(FileCategoryCreateRequestDto requestDto);
    Task<GenericResponse<GetFileCategoryResponseDto>> GetFileCategoryByIdAsync(int Id);
    Task<GenericResponse<IEnumerable<GetAllFileCategoryResponseDto>>> GetAllFileCategoriesAsync();
}