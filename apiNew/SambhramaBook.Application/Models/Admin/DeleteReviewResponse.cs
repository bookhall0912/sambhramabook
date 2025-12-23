using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class DeleteReviewResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ErrorResponse? Error { get; set; }
}

