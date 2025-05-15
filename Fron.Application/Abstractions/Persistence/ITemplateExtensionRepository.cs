using Fron.Domain.Dto.TemplateExtension;
using Fron.Domain.Entities;

namespace Fron.Application.Abstractions.Persistence;
public interface ITemplateExtensionRepository
{
    Task<TemplateExtension> CreateTemplateExtensionAsync(TemplateExtension entity);
    Task DeleteTemplateExtensionAsync(TemplateExtension entity);
    Task<TemplateExtension?> GetByIdAsync(int? id);
    Task<IEnumerable<GetAllTemplateExtensionsResponseDto>> GetAllTemplateExtensionsAsync();
    Task<TemplateExtension> UpdateTemplateExtensionAsync(TemplateExtension entity);
}