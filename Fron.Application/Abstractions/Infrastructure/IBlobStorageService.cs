using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Fron.Application.Abstractions.Infrastructure;
public interface IBlobStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string blobName, string blobContainerName);
    Task<string> UploadFileAsync(Stream stream, string blobName, string blobContainerName);
    Task<BlobDownloadInfo> DownloadFileAsync(string blobName, string blobContainerName);
    Task DeleteFileAsync(string blobName, string blobContainerName);
    Task<bool> DoesBlobExistAsync(string blobName, string blobContainerName);
}