using Fron.Application.Utility;
using Fron.Domain.AuthEntities;
using Fron.Domain.Dto.FileCategory;
using Fron.Domain.Dto.FileStorage;
using Fron.Domain.Dto.Login;
using Fron.Domain.Dto.Role;
using Fron.Domain.Dto.TemplateExtension;
using Fron.Domain.Dto.UserRegistration;
using Fron.Domain.Entities;

namespace Fron.Application.Mapping;
public static class DtoToEntityMappingExtensions
{
    public static User Map(this LoginRequestDto requestDto)
    {
        if (requestDto == null) { return new User(); }
        else return new User
        {
            Username = requestDto.UserName,
            Password = requestDto.Password
        };
    }

    public static User Map(this UserRegistrationRequestDto requestDto, string key)
    {
        if (requestDto == null) { return new User(); }
        else return new User
        {
            Name = Helper.Base64Decode(requestDto.Name),
            Username = Helper.Base64Decode(requestDto.UserName),
            Password = Helper.Encrypt(Helper.Base64Decode(requestDto.Password), key),
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };
    }

    public static Role Map(this RoleRegistrationRequestDto requestDto)
    {
        if (requestDto == null) { return new Role(); }
        else return new Role
        {
            Name = requestDto.Name,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };
    }

    public static Domain.Entities.FileCategory Map(this FileCategoryCreateRequestDto requestDto)
    {
        if (requestDto == null) { return new Domain.Entities.FileCategory(); }
        else return new Domain.Entities.FileCategory
        {
            Name = requestDto.FileCategory.ToString(),
            FileCategoryEnum = (int)requestDto.FileCategory,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };
    }

    public static Domain.Entities.TemplateExtension Map(this TemplateExtensionCreateRequestDto requestDto)
    {
        if (requestDto == null) { return new Domain.Entities.TemplateExtension(); }
        else return new Domain.Entities.TemplateExtension
        {
            Name = requestDto.TemplateExtension.ToString(),
            TemplateExtensionEnum = (int)requestDto.TemplateExtension,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };
    }

    public static FileStorage Map(this FileStorageCreateRequestDto requestDto)
    {
        if (requestDto == null) { return new FileStorage(); }
        else return new FileStorage
        {
            Name = requestDto.Name,
            FileExtension = requestDto.FileExtension,
            StorageUrl = requestDto.StorageUrl,
            Size = requestDto.Size,
            FileCategoryId = requestDto.FileCategoryId,
            Support = requestDto.Support,
            TemplateName = requestDto.TemplateName,
            TemplateExtensionId = requestDto.TemplateExtensionId,
            IsActive = true,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };
    }
}
