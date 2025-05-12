using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Infrastructure;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Mapping;
using Fron.Domain.AuthEntities;
using Fron.Domain.Constants;
using Fron.Domain.Dto.Role;
using Fron.Domain.GenericResponse;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Fron.Application.Services;
public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IDocumentService _documentService;

    public RoleService(
        IRoleRepository roleRepository,
        IDocumentService documentService)
    {
        _roleRepository = roleRepository;
        _documentService = documentService;
    }

    public async Task<GenericResponse<RoleRegistrationResponseDto>> CreateRoleAsync(RoleRegistrationRequestDto request)
    {
        if (request == null)
        {
            return GenericResponse<RoleRegistrationResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _roleRepository.CreateRoleAsync(request.Map());

                scope.Complete();

                return GenericResponse<RoleRegistrationResponseDto>.Success(
                result.Map(),
                ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);
            }
        }
    }

    public async Task<GenericResponse<IEnumerable<GetAllRolesResponseDto>>> GetAllRolesAsync()
    {
        var result = await _roleRepository.GetAllRolesAsync();

        if (result == null || result.Count() < 1)
        {
            return GenericResponse<IEnumerable<GetAllRolesResponseDto>>.Failure(
                ApiResponseMessages.RECORD_NOT_FOUND,
                ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<IEnumerable<GetAllRolesResponseDto>>.Success(
                result,
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<GetRoleResponseDto>> GetRoleByIdAsync(long Id)
    {
        var entity = await _roleRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse<GetRoleResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            return GenericResponse<GetRoleResponseDto>.Success(
                entity.MapGet(),
                ApiResponseMessages.RECORD_FETCHED_SUCCESSFULLY,
                ApiStatusCodes.RECORD_FETCHED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<UpdateRoleResponseDto>> UpdateRoleAsync(UpdateRoleRequestDto requestDto)
    {
        if (requestDto == null)
        {
            return GenericResponse<UpdateRoleResponseDto>.Failure(ApiResponseMessages.INVALID_RECORD, ApiStatusCodes.FAILED);
        }
        else
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.Serializable },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity = await _roleRepository.GetByIdAsync(requestDto.Id);

                if (entity == null)
                {
                    return GenericResponse<UpdateRoleResponseDto>.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.FAILED);
                }
                else
                {
                    entity.Map(requestDto);

                    var updatedEntity = await _roleRepository.UpdateRoleAsync(entity);

                    scope.Complete();

                    return GenericResponse<UpdateRoleResponseDto>.Success(
                        updatedEntity.MapUpdate(),
                        ApiResponseMessages.RECORD_UPDATED_SUCCESSFULLY,
                        ApiStatusCodes.RECORD_UPDATED_SUCCESSFULLY);
                }
            }
        }
    }

    public async Task<GenericResponse> DeleteRoleAsync(long Id)
    {
        var entity = await _roleRepository.GetByIdAsync(Id);

        if (entity == null)
        {
            return GenericResponse.Failure(ApiResponseMessages.RECORD_NOT_FOUND, ApiStatusCodes.RECORD_NOT_FOUND);
        }
        else
        {
            await _roleRepository.DeleteRoleAsync(entity);
            return GenericResponse.Success(ApiResponseMessages.RECORD_DELETED_SUCCESSFULLY, ApiStatusCodes.RECORD_DELETED_SUCCESSFULLY);
        }
    }

    public async Task<GenericResponse<RolesUploadResponseDto>> BulkInsertRolesAsync(IFormFile file)
    {
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            stream.Position = 0;

            List<string> propNameToSkip = new List<string>()
            {
                "Id",
                "UserRoles"
            };

            var data = _documentService.GetListFromExcelSheet<Role>(stream, propNameToSkip);

            data.Item2.Position = 0; //stream recieved
            string excelName = $"{FileNames.ROLES_UPLOAD_ERROR}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{FileExtensions.EXCEL}";

            var formFile = _documentService.CreateFormFileFromFile(data.Item2, MimeTypes.EXCEL, excelName);

            RolesUploadResponseDto responseDto = new RolesUploadResponseDto(excelName, formFile, MimeTypes.EXCEL);

            if (data.Item1 == null || data.Item1.Count < 1)
            {
                return GenericResponse<RolesUploadResponseDto>.Failure(
                    responseDto,
                    ApiResponseMessages.RECORD_NOT_FOUND,
                    ApiStatusCodes.RECORD_NOT_FOUND);
            }
            else
            {
                await _roleRepository.BulkInsertRolesAsync(data.Item1);
                return GenericResponse<RolesUploadResponseDto>.Success(
                    responseDto,
                    ApiResponseMessages.RECORD_SAVED_SUCCESSFULLY,
                    ApiStatusCodes.RECORD_SAVED_SUCCESSFULLY);
            }
        }
    }
}
