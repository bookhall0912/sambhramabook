using Microsoft.AspNetCore.Http;
using SambhramaBook.Application.Models.FileUpload;

namespace SambhramaBook.Application.Handlers.FileUpload;

public class UploadMultipleImagesHandler
{
    public UploadMultipleImagesHandler()
    {
    }

    public async Task<UploadMultipleImagesResponse> HandleAsync(List<IFormFile> images, CancellationToken cancellationToken = default)
    {
        var files = new List<UploadedFileDto>();

        foreach (var image in images)
        {
            // TODO: Upload to storage
            var fileId = Guid.NewGuid().ToString();
            files.Add(new UploadedFileDto
            {
                FileId = fileId,
                Url = $"https://example.com/uploaded-{fileId}.jpg"
            });
        }

        return new UploadMultipleImagesResponse
        {
            Success = true,
            Data = new UploadMultipleImagesResponseData
            {
                Files = files
            }
        };
    }
}

