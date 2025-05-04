using Microsoft.Data.SqlClient;

namespace Fron.Application.Abstractions.Persistence;
public interface IGenericSQLHelper
{
    Task<List<T>> ExecuteStoredProcedureAsync<T>(string sql, SqlParameter[] parameters, bool executeInAuthDB = false);
    Task ExecuteStoredProcedureAsync(string sql, SqlParameter[] parameters, bool executeInAuthDB = false);
    Task ExecuteStoredProcedureAsync(string sql, bool executeInAuthDB = false);
}
