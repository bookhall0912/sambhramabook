using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Models.FileUpload;
using SambhramaBook.Application.Services;

namespace SambhramaBook.Application.Handlers.FileUpload;

public class DeleteUploadedFileHandler
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly ILogger<DeleteUploadedFileHandler> _logger;

    public DeleteUploadedFileHandler(
        IBlobStorageService blobStorageService,
        ILogger<DeleteUploadedFileHandler> logger)
    {
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    public async Task<DeleteUploadedFileResponse> HandleAsync(string blobName, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(blobName))
            {
                return new DeleteUploadedFileResponse
                {
                    Success = false,
                    Message = "Blob name is required"
                };
            }

            await _blobStorageService.DeleteBlobAsync(blobName, cancellationToken);
            
            return new DeleteUploadedFileResponse
            {
                Success = true,
                Message = "File deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {BlobName}", blobName);
            return new DeleteUploadedFileResponse
            {
                Success = false,
                Message = "Failed to delete file"
            };
        }
    }
}

