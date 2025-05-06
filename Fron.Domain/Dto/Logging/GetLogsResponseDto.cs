namespace Fron.Domain.Dto.Logging;

public sealed record GetLogsResponseDto(
    int Id,
    string Exception,
    string Message,
    int StatusCode,
    string StackTrace,
    string? UserDescription,
    string RequestMethod,
    string RequestPath,
    string RequestHeaders,
    string Source);