﻿using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Fron.Application.Abstractions.Infrastructure;

public interface IDocumentService
{
    List<Dictionary<string, object>> GetListFromExcelSheet(Stream fileStream);
    (List<T>, Stream) GetListFromExcelSheet<T>(Stream fileStream, List<string>? propNameToSkip = null);
    List<T> GetListFromExcelSheet<T>(ExcelWorksheet sheet, List<string>? propNameToSkip = null);
    void GeneratePDFStream<T>(T model, string templateFileContent, string templateResourcesPath, Stream pdfStream);
    Task<IFormFile?> CreateFormFileFromFile(string filePath, string contentType);
    Task<IFormFile?> CreateFormFileFromFile(Stream stream, string contentType, string fileNameWithExtension);
    Task CreateFileFromFormFileAsync(IFormFile formFile, string filePath);
    Task CopyToFile(Stream stream, string path);
    Task<Stream> GetFileStreamAsync(string path);
    string GetFileNameFromPath(string filePath);
    string GetFileNameWithoutExtension(string filePath);
    string GetFileExtension(string fileNameWithExtension);
    string GetFilePath(string fileNameWithExtension, string directoryPath);
    string GetFilePathWithoutExtension(string fileNameWithExtension, string directoryPath);
    string GetFilePathBasedExecutingAssemblyPath(string partialPathWithFileExtension);
    string GetFileNameWithNewExtension(string fileNameWithExtension, string newExtension);
    string GetDestinationFilePath(string fileNameWithExtension, string destinationFileWithExtension);
    Task CreateDirectoryIfNotExists(string directoryPath);
    bool CheckFileExists(string filePath);
}