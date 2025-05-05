using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.Logging;

namespace Fron.Application.Abstractions.Persistence;
public interface ILoggingRepository
{
    Task<IEnumerable<GetLogsResponseDto>> GetExceptionLogsAsync(int? numberOfLogs, string? endPointName);
    Task LogExceptionAsync(LogEntryErrors entity);
}