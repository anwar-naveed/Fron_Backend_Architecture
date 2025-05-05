using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Infrastructure;
using Fron.Application.Abstractions.Persistence;
using Fron.Domain.AuthEntities;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Logging;
using Fron.Domain.GenericResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Fron.Infrastructure.Utility.Services;
public class LoggingService : ILoggingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<LoggingService> _logger;
    private readonly IUserResolverService _userResolverService;
    private readonly ILoggingRepository _loggingRepository;
    public LoggingService(IHttpContextAccessor httpContextAccessor,
        ILogger<LoggingService> logger,
        ILoggingRepository loggingRepository,
        IUserResolverService userResolverService)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _loggingRepository = loggingRepository;
        _userResolverService = userResolverService;
    }
    public async Task LogExceptionAsync(Exception ex)
    {
        //LogToAzureBlogStorage(ex); TODO.
        await LogToDatabaseAsync(ex);
    }

    public async Task<GenericResponse<IEnumerable<GetLogsResponseDto>>> GetExceptionLogsAsync(int? numberOfLogs, string? endPointName)
    {
        var logs = await _loggingRepository.GetExceptionLogsAsync(numberOfLogs, endPointName);
        return GenericResponse<IEnumerable<GetLogsResponseDto>>.Success(logs, ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY, ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
    }

    #region Private Methods

    private async Task LogToDatabaseAsync(Exception ex)
    {
        var entity = new LogEntryErrors()
        {
            Exception = ex.InnerException?.ToString() ?? ex.ToString(),
            Message = ex.Message,
            RequestHeaders = GetRequestHeaders(),
            RequestMethod = GetRequestMethod(),
            RequestPath = GetRequestPath(),
            StackTrace = ex.StackTrace ?? string.Empty,
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Source = ex.Source ?? string.Empty,
            UserDescription = _userResolverService.IsUserAuthenticated() ? $"UserId: {_userResolverService.GetUserId()} - Username: {_userResolverService.GetLoggedInUsername()}" : null
        };

        await _loggingRepository.LogExceptionAsync(entity);

    }
    private void LogToAzureBlogStorage(Exception ex)
        => _logger.LogError(ex, message: ex.Message);

    private string GetRequestHeaders()
        => Newtonsoft.Json.JsonConvert.SerializeObject(_httpContextAccessor.HttpContext!.Request.Headers);

    private string GetRequestMethod()
        => _httpContextAccessor.HttpContext!.Request.Method;

    private string GetRequestPath()
        => _httpContextAccessor.HttpContext!.Request.GetDisplayUrl();
    #endregion
}
