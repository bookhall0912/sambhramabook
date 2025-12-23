using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Notifications;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Application.Handlers.Notifications;

public class MarkAllNotificationsReadHandler
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    public MarkAllNotificationsReadHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<MarkAllNotificationsReadResponse> HandleAsync(long userId, CancellationToken cancellationToken = default)
    {
        var notifications = await _notificationRepository.GetUnreadByUserIdAsync(userId, cancellationToken);

        var now = _dateTimeProvider.GetUtcNow();
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadAt = now;
            // Notification doesn't have UpdatedAt property
            await _notificationRepository.UpdateAsync(notification, cancellationToken);
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        return new MarkAllNotificationsReadResponse
        {
            Success = true,
            Message = "All notifications marked as read"
        };
    }
}

