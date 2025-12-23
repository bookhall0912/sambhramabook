using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Reviews;

public class CreateReviewResponse
{
    public bool Success { get; set; }
    public CreateReviewResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class CreateReviewResponseData
{
    public string Id { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

