using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.FileUpload;
using SambhramaBook.Application.Services;

namespace SambhramaBook.Application.Handlers.FileUpload;

public class GenerateUploadTokenHandler
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger<GenerateUploadTokenHandler> _logger;

    public GenerateUploadTokenHandler(
        IBlobStorageService blobStorageService,
        ILogger<GenerateUploadTokenHandler> logger)
    {
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    public async Task<GenerateUploadTokenResponse> HandleAsync(GenerateUploadTokenRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.FileName))
            {
                return new GenerateUploadTokenResponse
                {
                    Success = false,
                    Error = new ErrorResponse { Code = "INVALID_REQUEST", Message = "FileName is required" }
                };
            }

            // Generate unique blob name
            var blobName = $"{Guid.NewGuid()}_{request.FileName}";

            // Generate SAS URL for direct upload
            var uploadUrl = await _blobStorageService.GenerateUploadSasUrlAsync(blobName, request.ContentType, cancellationToken);
            
            // Get public URL (for after upload)
            var publicUrl = _blobStorageService.GetBlobUrl(blobName);

            return new GenerateUploadTokenResponse
            {
                Success = true,
                Data = new GenerateUploadTokenResponseData
                {
                    UploadUrl = uploadUrl,
                    BlobName = blobName,
                    PublicUrl = publicUrl,
                    ExpiresInSeconds = 1800 // 30 minutes
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating upload token for file {FileName}", request.FileName);
            return new GenerateUploadTokenResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "INTERNAL_ERROR", Message = "Failed to generate upload token" }
            };
        }
    }
}

