namespace SambhramaBook.Application.Models.FileUpload;

public class GenerateUploadTokenRequest
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "image/jpeg";
}

