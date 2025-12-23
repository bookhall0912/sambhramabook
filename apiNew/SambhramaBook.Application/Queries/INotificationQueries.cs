using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Queries;

public interface INotificationQueries
{
    Task<(IEnumerable<Notification> Notifications, int Total)> GetNotificationsAsync(
        long userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(long userId, CancellationToken cancellationToken = default);
}

