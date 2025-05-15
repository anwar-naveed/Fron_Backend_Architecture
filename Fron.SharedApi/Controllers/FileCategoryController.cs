using Fron.Application.Abstractions.Application;
using Fron.Domain.Dto.FileCategory;
using Microsoft.AspNetCore.Mvc;

namespace Fron.SharedApi.Controllers;
[Route("api/file-category")]
public class FileCategoryController : BaseApiController
{
    private readonly IFileCategoryService _fileCategoryService;

    public FileCategoryController(IFileCategoryService fileCategoryService)
    {
        _fileCategoryService = fileCategoryService;
    }

    [HttpPost("File-Category-Create")]
    public async Task<IActionResult> CreateFileCategory([FromBody] FileCategoryCreateRequestDto requestDto)
        => Ok(await _fileCategoryService.CreateFileCategoryAsync(requestDto));

    [HttpGet("Get-All-File-Categories")]
    public async Task<IActionResult> GetAllFileCategoriesAsync()
        => Ok(await _fileCategoryService.GetAllFileCategoriesAsync());

    [HttpGet("Get-File-Category")]
    public async Task<IActionResult> GetFileCategoryByIdAsync(int Id)
        => Ok(await _fileCategoryService.GetFileCategoryByIdAsync(Id));
}
