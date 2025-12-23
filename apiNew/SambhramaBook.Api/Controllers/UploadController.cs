using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Handlers.FileUpload;
using SambhramaBook.Application.Models.FileUpload;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/upload")]
[Authorize]
public class UploadController : ControllerBase
{
    private readonly GenerateUploadTokenHandler _generateUploadTokenHandler;
    private readonly UploadImageHandler _uploadImageHandler;
    private readonly DeleteUploadedFileHandler _deleteUploadedFileHandler;
    private readonly ILogger<UploadController> _logger;

    public UploadController(
        GenerateUploadTokenHandler generateUploadTokenHandler,
        UploadImageHandler uploadImageHandler,
        DeleteUploadedFileHandler deleteUploadedFileHandler,
        ILogger<UploadController> logger)
    {
        _generateUploadTokenHandler = generateUploadTokenHandler;
        _uploadImageHandler = uploadImageHandler;
        _deleteUploadedFileHandler = deleteUploadedFileHandler;
        _logger = logger;
    }

    /// <summary>
    /// Generate an upload token (SAS URL) for direct upload to Azure Blob Storage
    /// </summary>
    [HttpPost("token")]
    [ProducesResponseType(typeof(GenerateUploadTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GenerateUploadTokenResponse>> GenerateUploadToken(
        [FromBody] GenerateUploadTokenRequest request,
        CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.FileName))
            {
                return BadRequest(new { success = false, message = "FileName is required" });
            }

            var response = await _generateUploadTokenHandler.HandleAsync(request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating upload token");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// Confirm an image upload after it has been uploaded directly to Azure Blob Storage
    /// Returns the public URL for the uploaded blob
    /// </summary>
    [HttpPost("image/confirm")]
    [ProducesResponseType(typeof(UploadImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UploadImageResponse>> ConfirmImageUpload(
        [FromBody] ConfirmUploadRequest request,
        CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.BlobName))
            {
                return BadRequest(new { success = false, message = "BlobName is required" });
            }

            var response = await _uploadImageHandler.HandleAsync(request.BlobName, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming image upload");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // Note: Multiple image uploads should use the token endpoint multiple times
    // or implement a batch token generation endpoint if needed

    [HttpDelete("{fileId}")]
    [ProducesResponseType(typeof(DeleteUploadedFileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteUploadedFileResponse>> DeleteUploadedFile(
        string fileId,
        CancellationToken ct)
    {
        try
        {
            var response = await _deleteUploadedFileHandler.HandleAsync(fileId, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}

