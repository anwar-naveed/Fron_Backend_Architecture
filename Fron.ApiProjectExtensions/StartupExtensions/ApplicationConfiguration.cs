using Fron.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Fron.Domain.Configuration;
using Syncfusion.Licensing;

namespace Fron.ApiProjectExtensions.StartupExtensions;
public static class ApplicationConfiguration
{
    private static string GetDevelopmentConfigurationRootPath(IWebHostEnvironment env)
    {
        string appsettingsDir = Environment.GetEnvironmentVariable("ASPNETCORE_APPSETTINGS_DIRECTORY")!;

        if (string.IsNullOrWhiteSpace(appsettingsDir))
        {
            return env.ContentRootPath;
        }

        var parent = Directory.GetParent(Directory.GetCurrentDirectory());
        return Path.Combine(parent!.FullName, appsettingsDir);
    }

    private static string GetConfigurationRootPath(IWebHostEnvironment env)
        => env.IsDevelopment() ? GetDevelopmentConfigurationRootPath(env) : env.ContentRootPath;

    public static IConfigurationBuilder CreateConfigurationBuilder(IWebHostEnvironment env)
        => CreateConfigurationBuilder(env, GetConfigurationRootPath(env));

    private static IConfigurationBuilder CreateConfigurationBuilder(IWebHostEnvironment env, string configurationRootPath)
        => new ConfigurationBuilder()
        .SetBasePath(configurationRootPath)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();

    private static IServiceCollection AddTokenValidationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["AuthenticationConfiguration:Issuer"],
            ValidAudience = configuration["AuthenticationConfiguration:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthenticationConfiguration:SecretKey"] ?? string.Empty)),
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwt =>
        {
            jwt.TokenValidationParameters = tokenValidationParameters;
        });

        return services;
    }

    private static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, string title, string version)
    {
        services.AddSwaggerGen(c =>
        {

            c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });


            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
            });
        });

        return services;
    }

    public static IServiceCollection AddDbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataDbContext>((provider, options) =>
        {
            //var tenantService = provider.GetRequiredService<TenantService>();
            options.UseSqlServer(configuration.GetConnectionString("DataDbConnection"), conf =>
            {
                conf.UseHierarchyId();
            });
        },
        ServiceLifetime.Scoped,
        ServiceLifetime.Singleton);

        services.AddDbContext<AuthDbContext>((provider, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("AuthDbConnection"), conf =>
            {
                conf.UseHierarchyId();
            });
        },
        ServiceLifetime.Scoped,
        ServiceLifetime.Singleton);

        return services;
    }


    private static IServiceCollection AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HashingConfiguration>(configuration.GetSection("HashingConfiguration"));
        services.Configure<DatabaseConfiguration>(configuration.GetSection("DatabaseConfiguration"));
        services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
        services.Configure<AuthenticationConfiguration>(configuration.GetSection("AuthenticationConfiguration"));
        services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
        services.Configure<EncryptionConfiguration>(configuration.GetSection("EncryptionConfiguration"));
        services.Configure<TemplateConfiguraion>(configuration.GetSection("TemplateConfiguraion"));
        services.Configure<OrganizationConfiguration>(configuration.GetSection("OrganizationConfiguration"));
        services.Configure<PdfConfiguration>(configuration.GetSection("PdfConfiguration"));
        services.Configure<NonTemplateConfiguration>(configuration.GetSection("NonTemplateConfiguration"));

        return services;
    }

    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration, string title, string version)
    {
        SyncfusionLicenseProvider.RegisterLicense(configuration.GetValue<string>("PdfConfiguration:SyncFusionLicenseKey"));

        services.AddApplicationServices();

        services.AddInfrastructureRepositories(configuration);

        services.AddTokenValidationConfiguration(configuration);

        services.AddSwaggerConfiguration(title, version);

        services.AddDbConfiguration(configuration);

        services.AddOptionsConfiguration(configuration);

        return services;
    }
}
