﻿using Fron.Application.Abstractions.Application;
using Fron.Application.Abstractions.Identity;
using Fron.Application.Abstractions.Infrastructure;
using Fron.Application.Abstractions.Persistence;
using Fron.Application.Services;
using Fron.Infrastructure.Identity.Services;
using Fron.Infrastructure.Persistence.Repositories;
using Fron.Infrastructure.Utility;
using Fron.Infrastructure.Utility.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fron.ApiProjectExtensions.StartupExtensions;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ILoggingService, LoggingService>();
        services.AddScoped<IUserResolverService, UserResolverService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IFileCategoryService, FileCategoryService>();
        services.AddScoped<ITemplateExtensionService, TemplateExtensionService>();

        return services;
    }

    public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IGenericSQLHelper, GenericSQLHelper>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ILoggingRepository, LoggingRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddSingleton<IBlobStorageService>(provider =>
            new BlobStorageService(configuration["BlobStorage:ConnectionString"]!));
        services.AddScoped<IFileCategoryRepository, FileCategoryRepository>();
        services.AddScoped<IFileStorageRepository, FileStorageRepository>();
        services.AddScoped<ITemplateExtensionRepository, TemplateExtensionRepository>();

        return services;
    }
}
