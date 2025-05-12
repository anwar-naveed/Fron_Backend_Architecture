@echo off
setlocal

rem Get the current directory (where the batch file is located)
cd ..
set currentDir=%cd%
echo Current Directory: %currentDir%
echo Current Directory: %CD%
rem Navigate to the directory containing the project file (assuming it's one level up)
cd /d "%currentDir%\.."

rem Run the scaffold command using the project file in the current directory
rem Set the output directory based on the batch file's directory

 echo "scaffolding for DATA DB"
rem Print the output directory path for verification
echo Output Directory: %outputDir%

 dotnet ef dbcontext scaffold "Data Source=DESKTOP-HIAGS2V;Initial Catalog=AdventureWorks;User ID=sa;Password=123;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer -o "../Fron.Domain/Entities" --context-dir "Persistence/Contexts" --force --no-pluralize --no-onconfiguring --namespace Fron.Domain.Entities --context DataDbContext --context-namespace Fron.Infrastructure.Persistence.Contexts
 
 echo "Successfully generated"

pause
