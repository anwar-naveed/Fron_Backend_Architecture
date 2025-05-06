using Fron.Application.Abstractions.Persistence;
using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.Logging;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Persistence.Repositories;
public class LoggingRepository : AuthRepository, ILoggingRepository
{
    public LoggingRepository(DataDbContext context,
        AuthDbContext authContext) : base(context, authContext)
    {
    }

    public async Task<IEnumerable<GetLogsResponseDto>> GetExceptionLogsAsync(int? numberOfLogs, string? endPointName)
    {
        var query = _authContext.LogEntryErrors
            .OrderByDescending(x => x.Id)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(endPointName))
            query = query.Where(x => x.RequestPath.Contains(endPointName));

        return await query
            .Take(numberOfLogs ?? 10)
            .Select(x => new GetLogsResponseDto(
                x.Id,
                x.Exception,
                x.Message,
                x.StatusCode,
                x.StackTrace,
                x.UserDescription,
                x.RequestMethod,
                x.RequestPath,
                x.RequestHeaders,
                x.Source))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task LogExceptionAsync(LogEntryErrors entity)
    {
        await _authContext.LogEntryErrors.AddAsync(entity);
        await _authContext.SaveChangesAsync();
    }
}
