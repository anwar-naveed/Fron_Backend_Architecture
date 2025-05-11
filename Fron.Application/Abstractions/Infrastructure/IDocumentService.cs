using OfficeOpenXml;

namespace Fron.Application.Abstractions.Infrastructure;

public interface IDocumentService
{
    List<Dictionary<string, object>> GetListFromExcelSheet(Stream fileStream);
    (List<T>, Stream) GetListFromExcelSheet<T>(Stream fileStream, List<string>? propNameToSkip = null);
    List<T> GetListFromExcelSheet<T>(ExcelWorksheet sheet, List<string>? propNameToSkip = null);
    Stream GeneratePDFStream<T>(T model, string templateFileContent, string templateResourcesPath, Stream pdfStream);
}