using Fron.Application.Abstractions.Persistence;
using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Fron.Infrastructure.Utility;
public class GenericSQLHelper : IGenericSQLHelper
{
    private readonly DataDbContext _dataDbContext;
    private readonly AuthDbContext _authDbContext;
    public GenericSQLHelper(DataDbContext dataDbContext,
        AuthDbContext authDbContext)
    {
        _dataDbContext = dataDbContext;
        _authDbContext = authDbContext;
    }

    public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string sql, SqlParameter[] parameters, bool executeInAuthDB = false)
    {
        return await (executeInAuthDB
             ? _authDbContext.Database.SqlQueryRaw<T>(sql, parameters).ToListAsync()
             : _dataDbContext.Database.SqlQueryRaw<T>(sql, parameters).ToListAsync());

    }

    public async Task ExecuteStoredProcedureAsync(string sql, SqlParameter[] parameters, bool executeInAuthDB = false)
    {
        try
        {
            await (executeInAuthDB
           ? _authDbContext.Database.ExecuteSqlRawAsync(sql, parameters)
           : _dataDbContext.Database.ExecuteSqlRawAsync(sql, parameters));
        }
        catch (Exception e)
        {

            throw;
        }
    }

    public async Task ExecuteStoredProcedureAsync(string sql, bool executeInAuthDB = false)
    {
        await (executeInAuthDB
            ? _authDbContext.Database.ExecuteSqlRawAsync(sql)
            : _dataDbContext.Database.ExecuteSqlRawAsync(sql));
    }
}
