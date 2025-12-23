using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Services;

namespace SambhramaBook.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly string _cdnUrl;
    private readonly int _sasTokenExpiryMinutes;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(
        IConfiguration configuration,
        ILogger<BlobStorageService> logger)
    {
        _logger = logger;
        var connectionString = configuration["AzureBlobStorage:ConnectionString"];
        _containerName = configuration["AzureBlobStorage:ContainerName"] ?? "sambhramabook-uploads";
        _cdnUrl = configuration["AzureBlobStorage:CdnUrl"] ?? string.Empty;
        _sasTokenExpiryMinutes = int.Parse(configuration["AzureBlobStorage:SasTokenExpiryMinutes"] ?? "30");

        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogWarning("Azure Blob Storage connection string is not configured. File uploads will not work.");
            _blobServiceClient = null!;
        }
        else
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }
    }

    public async Task<string> GenerateUploadSasUrlAsync(string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        if (_blobServiceClient == null)
        {
            throw new InvalidOperationException("Azure Blob Storage is not configured. Please set AzureBlobStorage:ConnectionString in appsettings.json");
        }

        try
        {
            // Ensure container exists
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

            // Generate unique blob name with timestamp
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            // Generate SAS token with write permissions using the blob client
            // This works with connection string authentication
            var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Write | BlobSasPermissions.Create, DateTimeOffset.UtcNow.AddMinutes(_sasTokenExpiryMinutes));
            return sasUri.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating SAS URL for file {FileName}", fileName);
            throw;
        }
    }

    public string GetBlobUrl(string blobName)
    {
        if (string.IsNullOrEmpty(blobName))
            return string.Empty;

        // If CDN URL is configured, use it; otherwise construct from blob storage
        if (!string.IsNullOrEmpty(_cdnUrl))
        {
            return $"{_cdnUrl.TrimEnd('/')}/{blobName}";
        }

        if (_blobServiceClient == null)
        {
            return string.Empty;
        }

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        return blobClient.Uri.ToString();
    }

    public async Task DeleteBlobAsync(string blobName, CancellationToken cancellationToken = default)
    {
        if (_blobServiceClient == null || string.IsNullOrEmpty(blobName))
            return;

        try
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting blob {BlobName}", blobName);
            throw;
        }
    }

}

