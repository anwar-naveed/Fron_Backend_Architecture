﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Fron.Application.Abstractions.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Fron.Infrastructure.Utility.Services;
public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(string connectionString)
    {
        _blobServiceClient = new BlobServiceClient(connectionString);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string blobName, string blobContainerName)
    {
        BlobContainerClient containerClient = GetContainerClient(blobContainerName);

        await containerClient.CreateIfNotExistsAsync();

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    public async Task<string> UploadFileAsync(Stream stream, string blobName, string blobContainerName)
    {
        BlobContainerClient containerClient = GetContainerClient(blobContainerName);

        await containerClient.CreateIfNotExistsAsync();

        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.UploadAsync(stream, overwrite: true);

        return blobClient.Uri.ToString();
    }

    public async Task<BlobDownloadInfo> DownloadFileAsync(string blobName, string blobContainerName)
    {
        BlobContainerClient containerClient = GetContainerClient(blobContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        BlobDownloadInfo download = await blobClient.DownloadAsync();
        return download;
    }

    public async Task DeleteFileAsync(string blobName, string blobContainerName)
    {
        BlobContainerClient containerClient = GetContainerClient(blobContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<bool> DoesBlobExistAsync(string blobName, string blobContainerName)
    {
        BlobContainerClient containerClient = GetContainerClient(blobContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        return await blobClient.ExistsAsync();
    }

    private BlobContainerClient GetContainerClient(string blobContainerName)
    {
        return _blobServiceClient.GetBlobContainerClient(blobContainerName);
    }
}
