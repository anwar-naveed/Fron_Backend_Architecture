using Fron.Application.Abstractions.Application;
using Fron.Domain.Dto.File;
using Microsoft.AspNetCore.Mvc;

namespace Fron.SharedApi.Controllers;

[Route("api/file")]
public class FileController : BaseApiController
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("File-Save")]
    public async Task<IActionResult> Create([FromForm] FileUploadRequestDto fileRequestDto)
        => Ok(await _fileService.FileSaveAsync(fileRequestDto));

    [HttpPost("File-Get")]
    public async Task<IActionResult> GetFileAsync(GetFileRequestDto requestDto)
        => Ok(await _fileService.GetFileAsync(requestDto));
}
