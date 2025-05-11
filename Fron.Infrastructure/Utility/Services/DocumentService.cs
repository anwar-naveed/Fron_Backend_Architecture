using Fron.Application.Abstractions.Infrastructure;
using Fron.Domain.Configuration;
using Microsoft.Extensions.Options;
using OfficeOpenXml;

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
}
