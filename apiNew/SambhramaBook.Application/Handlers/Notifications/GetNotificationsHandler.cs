using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Notifications;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Notifications;

public interface IGetNotificationsHandler
{
    Task<GetNotificationsResponse> Handle(long userId, GetNotificationsRequest request, CancellationToken ct);
}

public class GetNotificationsHandler : IGetNotificationsHandler
{
    private readonly INotificationQueries _notificationQueries;

    public GetNotificationsHandler(INotificationQueries notificationQueries)
    {
        _notificationQueries = notificationQueries;
    }

    public async Task<GetNotificationsResponse> Handle(long userId, GetNotificationsRequest request, CancellationToken ct)
    {
        var (notifications, total) = await _notificationQueries.GetNotificationsAsync(
            userId,
            request.Page,
            request.PageSize,
            ct);

        var notificationDtos = notifications.Select(n => new NotificationDto
        {
            Id = n.Id.ToString(),
            Title = n.Title,
            Message = n.Message,
            Type = n.NotificationType.ToString().ToLower(),
            Read = n.IsRead,
            Timestamp = n.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            ActionUrl = n.ActionUrl
        }).ToList();

        return new GetNotificationsResponse
        {
            Success = true,
            Data = notificationDtos,
            Pagination = new PaginationInfo
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
            }
        };
    }
}

