using Fron.Domain.Dto.TemplateExtension;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Application;
public interface ITemplateExtensionService
{
    Task<GenericResponse<TemplateExtensionCreateResponseDto>> CreateTemplateExtensionAsync(TemplateExtensionCreateRequestDto requestDto);
    Task<GenericResponse<GetTemplateExtensionResponseDto>> GetTemplateExtensionByIdAsync(int Id);
    Task<GenericResponse<IEnumerable<GetAllTemplateExtensionsResponseDto>>> GetAllTemplateExtensionsAsync();
}