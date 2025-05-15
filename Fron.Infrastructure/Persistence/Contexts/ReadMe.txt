For scaffolding to work install dotnet-ef globally

To install the dotnet-ef tool, run the following command in CMD:

.NET 9

dotnet tool install --global dotnet-ef --version 9.*
.NET 8

dotnet tool install --global dotnet-ef --version 8.*
.NET 7

dotnet tool install --global dotnet-ef --version 7.*
.NET 6

dotnet tool install --global dotnet-ef --version 6.*
.NET 5

dotnet tool install --global dotnet-ef --version 5.*
.NET Core 3

dotnet tool install --global dotnet-ef --version 3.*


For Migrations in CMD .Net CLI

dotnet ef migrations add AddingEFExtensions -o MyMigrationDirectoryName -n Z.MyMigrationNamespace -c Z.MyContextName -p Z.MyProjectName -s Z.MyStartupProjectName


For Migrations in Package Manager Console

Add-Migration [MigrationName] -OutputDir MyMigrationDirectoryName -Namespace Z.MyMigrationNamespace -Context Z.MyContextName -Project Z.MyProjectName -StartupProject Z.MyStartupProjectName

Add-Migration Migration07052025-1 -OutputDir Persistence\Migrations -Context Fron.Infrastructure.Persistence.Contexts.AuthDbContext -Project Infrastructure\Fron.Infrastructure -StartupProject Apis\Fron.ApiProjectExtensions

For checking of errors use command Add-Migration Check

For removing migration use command Remove-Migration -Context Fron.Insfrastructure.Persistence.Contexts.AuthDbContext

For updating database use commmand Update-Database -Context Fron.Infrastructure.Persistence.Contexts.AuthDbContext


For MSSQL

Wrap the column name in brackets like so, from becomes [from] for reserved keywords in MSSQL.

select [from] from table;
It is also possible to use the following (useful when querying multiple tables):

select table.[from] from table;


For File Storage in Db Use Below script or create entities accordingly for migration
Create Schema Documents;
Go

Create Table Documents.FileCategory (
	Id INT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL,
	FileCategoryEnum INT NOT NULL,
	IsActive BIT NOT NULL,
	CreatedOn DateTime NOT NULL,
	ModifiedOn DateTime NOT NULL
	CONSTRAINT PK_FileCategory
        PRIMARY KEY NONCLUSTERED (Id)
);
Go

Create Table Documents.TemplateExtension(
	Id INT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL,
	TemplateExtensionEnum INT NOT NULL,
	IsActive BIT NOT NULL,
	CreatedOn DateTime NOT NULL,
	ModifiedOn DateTime NOT NULL
	CONSTRAINT PK_TemplateExtension
        PRIMARY KEY NONCLUSTERED (Id)
);
Go

Create Table Documents.FileStorage(
	Id BIGINT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(100) NOT NULL,
	FileExtension NVARCHAR(10) NOT NULL,
	StorageUrl NVARCHAR(250) NOT NULL,
	Size BIGINT NOT NULL,
	FileCategoryId INT NOT NULL,
	Support BIT NOT NULL,
	TemplateName NVARCHAR(100) NULL,
	TemplateExtensionId INT NULL,
	IsActive BIT NOT NULL,
	CreatedOn DateTime NOT NULL,
	ModifiedOn DateTime NOT NULL
	CONSTRAINT PK_FileStorage
        PRIMARY KEY NONCLUSTERED (Id),
    CONSTRAINT FK_FileStorage_FileCategory FOREIGN KEY (FileCategoryId)
        REFERENCES Documents.FileCategory(Id),
	CONSTRAINT FK_FileStorage_TemplateExtension FOREIGN KEY (TemplateExtensionId)
        REFERENCES Documents.TemplateExtension(Id)
    ON DELETE CASCADE
    ON UPDATE CASCADE
);

