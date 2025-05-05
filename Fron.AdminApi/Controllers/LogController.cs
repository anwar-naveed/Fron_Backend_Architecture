using Fron.Application.Abstractions.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Fron.AdminApi.Controllers;
[Route("api/log")]
public class LogController : BaseApiController
{
    private readonly ILoggingService _loggingService;

    public LogController(ILoggingService loggingService)
	{
        _loggingService = loggingService;
    }

    [HttpGet("get-exception-logs")]
    public async Task<IActionResult> GetExceptionLogsAsync(int? numberOfLogs, string? endPointName)
        => Ok(await _loggingService.GetExceptionLogsAsync(numberOfLogs, endPointName));
}
