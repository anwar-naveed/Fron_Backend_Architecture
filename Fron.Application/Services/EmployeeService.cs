using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Utility;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Employee;
using Fron.Domain.GenericResponse;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace Fron.Application.Services;
public class EmployeeService : IEmployeeService
{
    private readonly IGenericSQLHelper _genericSQLHelper;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(
        IGenericSQLHelper genericSQLHelper,
        IEmployeeRepository employeeRepository)
    {
        _genericSQLHelper = genericSQLHelper;
        _employeeRepository = employeeRepository;
    }

    public async Task<GenericResponse> UpdateEmployeeHierarchyAsync(EmployeeHierarchyUpdateRequestDto request)
    {
        if (request == null ||
            request.BusinessEntityId < 1 ||
            string.IsNullOrEmpty(request.LoginId))
        {
            return GenericResponse.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
           new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable },
           TransactionScopeAsyncFlowOption.Enabled))
            {

                var entity = await _employeeRepository.GetByIdAsync(request.BusinessEntityId);

                if (entity == null)
                {
                    return GenericResponse.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
                }
                else
                {
                    SqlParameter[] parameters = new[]
                    {
                        Helper.CreateInputParameter(SPParams.BUSINESS_ENTITY_ID,SqlDbType.VarChar, value: request.BusinessEntityId),
                        Helper.CreateInputParameter(SPParams.ORGANIZATION_NODE, SqlDbType.NVarChar, value: request.OrganizationNode),
                        Helper.CreateInputParameter(SPParams.LOGIN_ID, SqlDbType.NVarChar, value: $"adventure-works\\{request.LoginId}"),
                        Helper.CreateInputParameter(SPParams.JOB_TITLE, SqlDbType.NVarChar, value: request.JobTitle),
                        Helper.CreateInputParameter(SPParams.HIRE_DATE, SqlDbType.DateTime, value: request.HireDate),
                        Helper.CreateInputParameter(SPParams.CURRENT_FLAG, SqlDbType.Bit, value: request.CurrentFlag)
                    };

                    string spExecuteCommand = $@"
                    {StoreProcedureNames.EMPLOYEE_HIERARCHY_UPDATE}
                    {SPParams.BUSINESS_ENTITY_ID},
                    {SPParams.ORGANIZATION_NODE},
                    {SPParams.LOGIN_ID},
                    {SPParams.JOB_TITLE},
                    {SPParams.HIRE_DATE},
                    {SPParams.CURRENT_FLAG}
                    ";

                    await _genericSQLHelper.ExecuteStoredProcedureAsync(spExecuteCommand, parameters);
                    scope.Complete();
                    return GenericResponse.Success(ApiResponseMessages.RECORD_UPDATED_SUCCESSFULLY, ApiStatusCodes.RECORD_UPDATED_SUCCESSFULLY);
                }
            }
        }
    }
}
