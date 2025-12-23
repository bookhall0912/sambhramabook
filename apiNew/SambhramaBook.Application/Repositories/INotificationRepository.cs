using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetByUserIdAsync(long userId, bool? isRead = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<Notification> CreateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task<Notification> UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(long notificationId, CancellationToken cancellationToken = default);
    Task MarkAllAsReadAsync(long userId, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(long userId, CancellationToken cancellationToken = default);
}

