using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class PublishReviewResponse
{
    public bool Success { get; set; }
    public PublishReviewResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class PublishReviewResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

