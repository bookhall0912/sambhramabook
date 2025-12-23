namespace SambhramaBook.Application.Models.FileUpload;

public class UploadMultipleImagesResponse
{
    public bool Success { get; set; }
    public UploadMultipleImagesResponseData? Data { get; set; }
}

public class UploadMultipleImagesResponseData
{
    public List<UploadedFileDto> Files { get; set; } = [];
}

public class UploadedFileDto
{
    public string FileId { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

