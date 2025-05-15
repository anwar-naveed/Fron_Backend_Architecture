using Fron.Application.Abstractions.Application;
using Fron.Domain.Dto.FileStorage;
using Microsoft.AspNetCore.Mvc;

namespace Fron.SharedApi.Controllers;
[Route("api/file-storage")]
public class FileStorageController : BaseApiController
{
    private readonly IFileStorageService _fileStorageService;

    public FileStorageController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpPost("File-Storage-Create")]
    public async Task<IActionResult> CreateFileStorage([FromBody] FileStorageCreateRequestDto requestDto)
        => Ok(await _fileStorageService.CreateFileStorageAsync(requestDto));

    [HttpGet("Get-All-File-Storages")]
    public async Task<IActionResult> GetAllFileStoragesAsync()
        => Ok(await _fileStorageService.GetAllFileStoragesAsync());

    [HttpGet("Get-File-Storage")]
    public async Task<IActionResult> GetFileStorageByIdAsync(long Id)
        => Ok(await _fileStorageService.GetFileStorageByIdAsync(Id));
}
