using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Notifications;

public class MarkAllNotificationsReadResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ErrorResponse? Error { get; set; }
}

