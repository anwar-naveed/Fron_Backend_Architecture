using Fron.Application.Abstractions.Application;
using Fron.Domain.Dto.TemplateExtension;
using Microsoft.AspNetCore.Mvc;

namespace Fron.SharedApi.Controllers;
[Route("api/template-extension")]
public class TemplateExtensionController : BaseApiController
{
    private readonly ITemplateExtensionService _templateExtensionService;

    public TemplateExtensionController(ITemplateExtensionService templateExtensionService)
    {
        _templateExtensionService = templateExtensionService;
    }

    [HttpPost("Template-Extension-Create")]
    public async Task<IActionResult> CreateTemplateExtension([FromBody] TemplateExtensionCreateRequestDto requestDto)
        => Ok(await _templateExtensionService.CreateTemplateExtensionAsync(requestDto));

    [HttpGet("Get-All-Template-Extensions")]
    public async Task<IActionResult> GetAllTemplateExtensionsAsync()
        => Ok(await _templateExtensionService.GetAllTemplateExtensionsAsync());

    [HttpGet("Get-Template-Extension")]
    public async Task<IActionResult> GetTemplateExtensionByIdAsync(int Id)
        => Ok(await _templateExtensionService.GetTemplateExtensionByIdAsync(Id));
}
