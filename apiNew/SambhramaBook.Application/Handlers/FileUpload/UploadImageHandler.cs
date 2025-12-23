using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Models.FileUpload;
using SambhramaBook.Application.Services;

namespace SambhramaBook.Application.Handlers.FileUpload;

public class UploadImageHandler
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger<UploadImageHandler> _logger;

    public UploadImageHandler(
        IBlobStorageService blobStorageService,
        ILogger<UploadImageHandler> logger)
    {
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    public async Task<UploadImageResponse> HandleAsync(string blobName, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(blobName))
            {
                return new UploadImageResponse
                {
                    Success = false
                };
            }

            // Get the public URL for the uploaded blob
            var publicUrl = _blobStorageService.GetBlobUrl(blobName);

            return new UploadImageResponse
            {
                Success = true,
                Data = new UploadImageResponseData
                {
                    FileId = blobName,
                    Url = publicUrl,
                    ThumbnailUrl = publicUrl // In production, you might generate a thumbnail
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing uploaded image {BlobName}", blobName);
            return new UploadImageResponse
            {
                Success = false
            };
        }
    }
}

