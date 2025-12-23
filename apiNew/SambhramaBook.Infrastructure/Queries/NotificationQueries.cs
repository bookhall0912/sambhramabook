using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Queries;

public class NotificationQueries : INotificationQueries
{
    private readonly SambhramaBookDbContext _context;

    public NotificationQueries(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Notification> Notifications, int Total)> GetNotificationsAsync(
        long userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == userId)
            .AsNoTracking();

        var total = await query.CountAsync(cancellationToken);
        var notifications = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (notifications, total);
    }

    public async Task<int> GetUnreadCountAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .AsNoTracking()
            .CountAsync(cancellationToken);
    }
}

