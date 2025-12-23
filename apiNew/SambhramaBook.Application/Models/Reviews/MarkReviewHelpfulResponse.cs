using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Reviews;

public class MarkReviewHelpfulResponse
{
    public bool Success { get; set; }
    public MarkReviewHelpfulResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class MarkReviewHelpfulResponseData
{
    public string Id { get; set; } = string.Empty;
    public int HelpfulCount { get; set; }
    public string Message { get; set; } = string.Empty;
}

