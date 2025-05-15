using Fron.Domain.AuthEntities;
using Fron.Domain.Constants;
using Fron.Domain.Dto.FileCategory;
using Fron.Domain.Dto.FileStorage;
using Fron.Domain.Dto.Login;
using Fron.Domain.Dto.Role;
using Fron.Domain.Dto.TemplateExtension;
using Fron.Domain.Dto.User;
using Fron.Domain.Dto.UserRegistration;
using Fron.Domain.Entities;

namespace Fron.Application.Mapping;
public static class EntityToDtoMappingExtensions
{
    public static LoginResponseDto Map(this User a, string token)
    {
        if (a == null && !string.IsNullOrEmpty(token))
        {
            return new LoginResponseDto(
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            [],
            "",
            "",
            "");
        }
        else return new LoginResponseDto(
            "",
            "",
            "",
            "",
            a!.Username,
            "",
            "",
            "",
            [],
            "",
            "",
            token
        );
    }

    public static UserRegistrationResponseDto Map(this User user)
    {
        if (user == null)
        {
            return new UserRegistrationResponseDto(
            0,
            "",
            "",
            true,
            DateTime.UtcNow,
            DateTime.UtcNow);
        }
        else return new UserRegistrationResponseDto(
            user.Id,
            user.Name!,
            user.Username!,
            user.IsActive,
            user.CreatedOn,
            user.ModifiedOn
        );
    }

    public static RoleRegistrationResponseDto Map(this Role role)
    {
        if (role == null)
        {
            return new RoleRegistrationResponseDto(
            0,
            "",
            true,
            DateTime.UtcNow,
            DateTime.UtcNow);
        }
        else return new RoleRegistrationResponseDto(
            role.Id,
            role.Name!,
            role.IsActive,
            role.CreatedOn,
            role.ModifiedOn
        );
    }

    public static UpdateRoleResponseDto MapUpdate(this Role role)
    {
        if (role == null)
        {
            return new UpdateRoleResponseDto(
                0,
                "",
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new UpdateRoleResponseDto(
            role.Id,
            role.Name!,
            role.IsActive,
            role.CreatedOn,
            role.ModifiedOn);
    }

    public static GetRoleResponseDto MapGet(this Role role)
    {
        if (role == null)
        {
            return new GetRoleResponseDto(
                0,
                "",
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new GetRoleResponseDto(
            role.Id,
            role.Name!,
            role.IsActive,
            role.CreatedOn,
            role.ModifiedOn);
    }

    public static GetUserResponseDto MapGet(this User user)
    {
        if (user == null)
        {
            return new GetUserResponseDto(
                0,
                "",
                "",
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new GetUserResponseDto(
            user.Id,
            user.Name!,
            user.Username!,
            user.IsActive,
            user.CreatedOn,
            user.ModifiedOn);
    }

    public static UpdateUserResponseDto MapUpdate(this User user)
    {
        if (user == null)
        {
            return new UpdateUserResponseDto(
                0,
                "",
                "",
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new UpdateUserResponseDto(
            user.Id,
            user.Name!,
            user.Username!,
            user.IsActive,
            user.CreatedOn,
            user.ModifiedOn);
    }

    public static FileCategoryCreateReponseDto Map(this Domain.Entities.FileCategory fileCategory)
    {
        if (fileCategory == null)
        {
            return new FileCategoryCreateReponseDto(
            0,
            "",
            0,
            true,
            DateTime.UtcNow,
            DateTime.UtcNow);
        }
        else return new FileCategoryCreateReponseDto(
            fileCategory.Id,
            fileCategory.Name,
            fileCategory.FileCategoryEnum,
            fileCategory.IsActive,
            fileCategory.CreatedOn,
            fileCategory.ModifiedOn
        );
    }

    public static TemplateExtensionCreateResponseDto Map(this Domain.Entities.TemplateExtension templateExtension)
    {
        if (templateExtension == null)
        {
            return new TemplateExtensionCreateResponseDto(
            0,
            "",
            0,
            true,
            DateTime.UtcNow,
            DateTime.UtcNow);
        }
        else return new TemplateExtensionCreateResponseDto(
            templateExtension.Id,
            templateExtension.Name,
            templateExtension.TemplateExtensionEnum,
            templateExtension.IsActive,
            templateExtension.CreatedOn,
            templateExtension.ModifiedOn
        );
    }

    public static FileStorageCreateResponseDto Map(this FileStorage fileStorage)
    {
        if (fileStorage == null)
        {
            return new FileStorageCreateResponseDto(
            0,
            "",
            "",
            "",
            0,
            0,
            "",
            true,
            "",
            0,
            "",
            true,
            DateTime.UtcNow,
            DateTime.UtcNow);
        }
        else return new FileStorageCreateResponseDto(
            fileStorage.Id,
            fileStorage.Name,
            fileStorage.FileExtension,
            fileStorage.StorageUrl,
            fileStorage.Size,
            fileStorage.FileCategoryId,
            fileStorage.FileCategory != null ? fileStorage.FileCategory.Name : "",
            fileStorage.Support,
            fileStorage.TemplateName,
            fileStorage.TemplateExtensionId,
            fileStorage.TemplateExtension != null ? fileStorage.TemplateExtension.Name : "",
            fileStorage.IsActive,
            fileStorage.CreatedOn,
            fileStorage.ModifiedOn
        );
    }

    public static GetFileCategoryResponseDto MapGet(this Domain.Entities.FileCategory fileCategory)
    {
        if (fileCategory == null)
        {
            return new GetFileCategoryResponseDto(
                0,
                "",
                0,
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new GetFileCategoryResponseDto(
            fileCategory.Id,
            fileCategory.Name!,
            fileCategory.FileCategoryEnum,
            fileCategory.IsActive,
            fileCategory.CreatedOn,
            fileCategory.ModifiedOn);
    }

    public static GetTemplateExtensionResponseDto MapGet(this Domain.Entities.TemplateExtension templateExtension)
    {
        if (templateExtension == null)
        {
            return new GetTemplateExtensionResponseDto(
                0,
                "",
                0,
                true,
                DateTime.UtcNow,
                DateTime.UtcNow);
        }
        else return new GetTemplateExtensionResponseDto(
            templateExtension.Id,
            templateExtension.Name!,
            templateExtension.TemplateExtensionEnum,
            templateExtension.IsActive,
            templateExtension.CreatedOn,
            templateExtension.ModifiedOn);
    }

    public static GetFileStorageResponseDto MapGet(this FileStorage fileStorage)
    {
        if (fileStorage == null)
        {
            return new GetFileStorageResponseDto(
            0,
            "",
            "",
            "",
            0,
            0,
            true,
            "",
            0,
            true,
            DateTime.UtcNow,
            DateTime.UtcNow);
        }
        else return new GetFileStorageResponseDto(
            fileStorage.Id,
            fileStorage.Name,
            fileStorage.FileExtension,
            fileStorage.StorageUrl,
            fileStorage.Size,
            fileStorage.FileCategoryId,
            fileStorage.Support,
            fileStorage.TemplateName,
            fileStorage.TemplateExtensionId,
            fileStorage.IsActive,
            fileStorage.CreatedOn,
            fileStorage.ModifiedOn
        );
    }
}
