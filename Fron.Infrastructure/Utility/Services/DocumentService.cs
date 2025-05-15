using Fron.Application.Abstractions.Infrastructure;
using Fron.Domain.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using Scriban;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.Reflection;

namespace Fron.Infrastructure.Utility.Services;
public class DocumentService : IDocumentService
{
    private readonly OrganizationConfiguration _organizationConfiguration;

    public DocumentService(IOptions<OrganizationConfiguration> organizationConfiguration)
    {
        _organizationConfiguration = organizationConfiguration.Value;
    }

    public List<Dictionary<string, object>> GetListFromExcelSheet(Stream fileStream)
    {
        var result = new List<Dictionary<string, object>>();

        ExcelPackage.License.SetNonCommercialOrganization(_organizationConfiguration.Name);

        using (var package = new ExcelPackage(fileStream))
        {
            var worksheet = package.Workbook.Worksheets[0];
            var columnNames = new List<string>();
            var rowCount = worksheet.Dimension.Rows;
            var colCount = worksheet.Dimension.Columns;

            for (int col = 1; col <= colCount; col++)
            {
                columnNames.Add(worksheet.Cells[1, col].Text);
            }
            for (int row = 2; row <= rowCount; row++)
            {
                var rowData = new Dictionary<string, object>();
                for (int col = 1; col <= colCount; col++)
                {
                    var cellValue = worksheet.Cells[row, col].Text;
                    rowData[columnNames[col - 1]] = cellValue;
                }
                result.Add(rowData);
            }
        }
        return result;
    }

    public (List<T>, Stream) GetListFromExcelSheet<T>(Stream fileStream, List<string>? propNameToSkip = null)
    {
        List<T> list = new List<T>();
        var stream = new MemoryStream();

        ExcelPackage.License.SetNonCommercialOrganization(_organizationConfiguration.Name);

        ExcelPackage excelPackage = new ExcelPackage();
        ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("ErrorRows");
        List<int> errorRows = new List<int>();

        using (var package = new ExcelPackage(fileStream))
        {
            var sheet = package.Workbook.Worksheets[0];

            //first row is for knowing the properties of object
            var columnInfo = Enumerable.Range(1, sheet.Dimension.Columns)
                .ToList()
                .Select(n => new { Index = n, ColumnName = sheet.Cells[1, n].Value.ToString(), Format = sheet.Cells[1, n].Style.Numberformat });

            for (int row = 2; row < sheet.Dimension.Rows + 1; row++)
            {
                T obj = (T)Activator.CreateInstance(typeof(T))!;//generic object
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (propNameToSkip != null && propNameToSkip.Count > 0 && propNameToSkip.Contains(prop.Name))
                        continue;
                    else
                    {
                        int col = columnInfo.SingleOrDefault(c => c.ColumnName == prop.Name)!.Index;
                        var val = sheet.Cells[row, col].Value;
                        try
                        {
                            prop.SetValue(obj, Convert.ChangeType(val, prop.PropertyType));
                        }
                        catch (Exception)
                        {
                            errorRows.Add(row);
                            goto jump;
                        }
                    }
                }
                list.Add(obj);
            jump:;
            }

            foreach (var colHead in columnInfo)
            {
                excelWorksheet.Cells[1, colHead.Index].Value = colHead.ColumnName;
                excelWorksheet.Column(colHead.Index).Style.Numberformat.Format = colHead.Format.Format;
            }

            excelWorksheet.Row(1).Style.Font.Bold = true;

            if (errorRows.Count > 0)
            {
                int rowStartNum = 2;
                foreach (var rowNum in errorRows)
                {
                    foreach (var colHead in columnInfo)
                    {
                        excelWorksheet.Cells[rowStartNum, colHead.Index].Value = sheet.Cells[rowNum, colHead.Index].Value;
                    }
                    rowStartNum++;
                }
            }

            foreach (var colHead in columnInfo)
            {
                excelWorksheet.Column(colHead.Index).AutoFit();
            }
        }

        excelPackage.SaveAsAsync(stream);
        excelPackage.Dispose();

        return new(list, stream);
    }

    public List<T> GetListFromExcelSheet<T>(ExcelWorksheet sheet, List<string>? propNameToSkip = null)
    {
        List<T> list = new List<T>();

        //first row is for knowing the properties of object
        var columnInfo = Enumerable.Range(1, sheet.Dimension.Columns)
            .ToList()
            .Select(n => new { Index = n, ColumnName = sheet.Cells[1, n].Value.ToString(), Format = sheet.Cells[1, n].Style.Numberformat });

        for (int row = 2; row < sheet.Dimension.Rows + 1; row++)
        {
            T obj = (T)Activator.CreateInstance(typeof(T))!; //generic object
            foreach (var prop in typeof(T).GetProperties())
            {
                if (propNameToSkip != null && propNameToSkip.Count > 0 && propNameToSkip.Contains(prop.Name))
                    continue;
                else
                {
                    int col = columnInfo.SingleOrDefault(c => c.ColumnName == prop.Name)!.Index;
                    var val = sheet.Cells[row, col].Value;
                    var propType = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(val, propType));
                }
            }
            list.Add(obj);
        }

        return list;
    }

    public void GeneratePDFStream<T>(T model, string templateFileContent, string templateResourcesPath, Stream pdfStream)
    {
        //Load html template
        var template = Template.Parse(templateFileContent);
        var templateData = new { model };
        //Fill template with real invoice data
        var renderedContent = template.Render(templateData);
        //Convert html to PDF
        ConvertToPDFStream(renderedContent, templateResourcesPath, pdfStream);
    }


    private void ConvertToPDFStream(string htmlContent, string templateResourcesPath, Stream pdfStream)
    {

        //Initialize HTML to PDF converter with Blink rendering engine
        //HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

        HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();

        BlinkConverterSettings blinkConverterSettings = new BlinkConverterSettings();

        blinkConverterSettings.HtmlEncoding = System.Text.Encoding.UTF8;
        blinkConverterSettings.PdfPageSize = PdfPageSize.A4;
        blinkConverterSettings.EnableAutoScaling = true;

        htmlConverter.ConverterSettings = blinkConverterSettings;
        htmlConverter.ConverterSettings.Orientation = PdfPageOrientation.Portrait;

        //Convert HTML string to PDF
        PdfDocument document = htmlConverter.Convert(htmlContent, templateResourcesPath);

        //FileStream fs = new FileStream("Output.pdf", FileMode.OpenOrCreate, FileAccess.ReadWrite);

        //Save and close the PDF document

        //document.Save(fs);
        document.Save(pdfStream);
        document.Close(true);
        document.Dispose();
    }

    public async Task<IFormFile?> CreateFormFileFromFile(string filePath, string contentType)
    {
        try
        {
            // Read the file into a byte array
            byte[] fileBytes = File.ReadAllBytes(filePath);

            // Create a memory stream from the byte array
            MemoryStream memoryStream = new MemoryStream(fileBytes);

            // Create the FormFile object
            IFormFile formFile = new FormFile(memoryStream, 0, fileBytes.Length, "file", GetFileNameFromPath(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return await Task.FromResult<IFormFile?>(formFile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating the FormFile: {ex.Message}");
            return null;
        }
    }

    public async Task<IFormFile?> CreateFormFileFromFile(Stream stream, string contentType, string fileNameWithExtension)
    {
        try
        {
            // Create the FormFile object
            var formFile = new FormFile(stream, 0, stream.Length, "file", fileNameWithExtension)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return await Task.FromResult<IFormFile?>(formFile);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating the FormFile: {ex.Message}");
            return null;
        }
    }

    public async Task CreateFileFromFormFileAsync(IFormFile formFile, string filePath)
    {
        try
        {
            using var stream = new FileStream(filePath, FileMode.Create);
            await formFile.CopyToAsync(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating the file: {ex.Message}");
        }
    }

    public async Task CopyToFile(Stream stream, string path)
    {
        using var fileStream = File.Create(path);
        //stream.Position = 0;
        stream.CopyTo(fileStream);
        await Task.CompletedTask;
    }

    public async Task<Stream> GetFileStreamAsync(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        return await Task.FromResult(fileInfo.OpenRead());
    }

    public string GetFileNameFromPath(string filePath)
    {
        return Path.GetFileName(filePath);
    }

    public string GetFileNameWithoutExtension(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    public string GetFileExtension(string fileNameWithExtension)
    {
        return Path.GetExtension(fileNameWithExtension);
    }

    public string GetFilePath(string fileNameWithExtension, string directoryPath)
    {
        return Path.Combine(directoryPath, fileNameWithExtension);
    }

    public string GetFilePathWithoutExtension(string fileNameWithExtension, string directoryPath)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithExtension);
        return Path.Combine(directoryPath, fileNameWithoutExtension);
    }

    public string GetFilePathBasedExecutingAssemblyPath(string partialPathWithFileExtension)
    {
        return GetFilePath(partialPathWithFileExtension, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
    }

    public string GetFileNameWithNewExtension(string fileNameWithExtension, string newExtension)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithExtension);
        return Path.ChangeExtension(fileNameWithoutExtension, newExtension);
    }

    public string GetDestinationFilePath(string fileNameWithExtension, string destinationFileWithExtension)
    {
        FileInfo executeProject = new FileInfo(fileNameWithExtension);
        string basePath = Directory.GetCurrentDirectory();
        string destinationPhysicalFile = basePath.Replace(executeProject.Directory!.Name, destinationFileWithExtension);
        destinationPhysicalFile = destinationPhysicalFile.Replace("..", "").Replace("/", "\\").Replace("\\\\", "\\");
        return destinationPhysicalFile;
    }

    public async Task CreateDirectoryIfNotExists(string directoryPath)
    {
        Directory.CreateDirectory(directoryPath);
        await Task.CompletedTask;
    }

    public bool CheckFileExists(string filePath)
    {
        return File.Exists(filePath);
    }
}
