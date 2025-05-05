using Fron.Domain.Dto.Logging;
using Fron.Domain.GenericResponse;

namespace Fron.Application.Abstractions.Infrastructure;
public interface ILoggingService
{
    Task<GenericResponse<IEnumerable<GetLogsResponseDto>>> GetExceptionLogsAsync(int? numberOfLogs, string? endPointName);
    Task LogExceptionAsync(Exception ex);
}