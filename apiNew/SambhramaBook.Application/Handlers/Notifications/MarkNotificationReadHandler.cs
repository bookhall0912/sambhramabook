using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Notifications;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Notifications;

public class MarkNotificationReadHandler
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkNotificationReadHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<MarkNotificationReadResponse> HandleAsync(long userId, long notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId, cancellationToken);
        if (notification == null || notification.UserId != userId)
        {
            return new MarkNotificationReadResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Notification not found" }
            };
        }

        notification.IsRead = true;
        notification.ReadAt = _dateTimeProvider.GetUtcNow();
        // Notification doesn't have UpdatedAt property

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new MarkNotificationReadResponse
        {
            Success = true,
            Message = "Notification marked as read"
        };
    }
}

