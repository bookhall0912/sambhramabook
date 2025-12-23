namespace SambhramaBook.Application.Models.Notifications;

public class GetNotificationsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

