using SambhramaBook.Application.Models.Notifications;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Notifications;

public interface IGetUnreadCountHandler
{
    Task<UnreadCountResponse> Handle(long userId, CancellationToken ct);
}

public class GetUnreadCountHandler : IGetUnreadCountHandler
{
    private readonly INotificationQueries _notificationQueries;

    public GetUnreadCountHandler(INotificationQueries notificationQueries)
    {
        _notificationQueries = notificationQueries;
    }

    public async Task<UnreadCountResponse> Handle(long userId, CancellationToken ct)
    {
        var unreadCount = await _notificationQueries.GetUnreadCountAsync(userId, ct);

        return new UnreadCountResponse
        {
            Success = true,
            Data = new UnreadCountData
            {
                UnreadCount = unreadCount
            }
        };
    }
}

