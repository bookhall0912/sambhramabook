using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.FileUpload;

public class GenerateUploadTokenResponse
{
    public bool Success { get; set; }
    public GenerateUploadTokenResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class GenerateUploadTokenResponseData
{
    public string UploadUrl { get; set; } = string.Empty;
    public string BlobName { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;
    public int ExpiresInSeconds { get; set; }
}

