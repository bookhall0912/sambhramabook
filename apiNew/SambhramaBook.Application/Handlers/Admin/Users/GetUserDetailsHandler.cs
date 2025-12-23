using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Admin.Users;

public interface IGetUserDetailsHandler : IQueryHandler<long, AdminUserDetailDto?>;

public class GetUserDetailsHandler : IGetUserDetailsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetUserDetailsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<AdminUserDetailDto?> Handle(long id, CancellationToken ct)
    {
        var user = await _adminQueries.GetUserDetailsAsync(id, ct);
        if (user == null)
            return null;

        var bookings = await _adminQueries.GetUserBookingCountAsync(id, ct);
        var totalSpent = await _adminQueries.GetUserTotalSpentAsync(id, ct);

        return new AdminUserDetailDto
        {
            Id = user.Id.ToString(),
            Name = user.Name,
            Email = user.Email,
            Mobile = user.Mobile,
            Avatar = user.UserProfile?.ProfileImageUrl,
            Status = user.IsActive ? "ACTIVE" : "INACTIVE",
            JoinDate = user.CreatedAt.ToString("yyyy-MM-dd"),
            Bookings = bookings,
            TotalSpent = totalSpent,
            LastLogin = user.LastLoginAt?.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
    }
}

