using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Notifications;

public class GetNotificationsResponse
{
    public bool Success { get; set; }
    public List<NotificationDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class NotificationDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // booking | payment | reminder | system
    public bool Read { get; set; }
    public string Timestamp { get; set; } = string.Empty;
    public string? ActionUrl { get; set; }
}

