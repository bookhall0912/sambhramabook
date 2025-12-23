namespace SambhramaBook.Application.Models.FileUpload;

public class UploadImageResponse
{
    public bool Success { get; set; }
    public UploadImageResponseData? Data { get; set; }
}

public class UploadImageResponseData
{
    public string FileId { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
}

