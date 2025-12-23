namespace SambhramaBook.Application.Models.Notifications;

public class UnreadCountResponse
{
    public bool Success { get; set; }
    public UnreadCountData? Data { get; set; }
}

public class UnreadCountData
{
    public int UnreadCount { get; set; }
}

