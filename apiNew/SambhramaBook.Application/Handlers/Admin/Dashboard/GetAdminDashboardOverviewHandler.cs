using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Dashboard;

public interface IGetAdminDashboardOverviewHandler : IQueryHandler<AdminDashboardOverviewResponse>;

public class GetAdminDashboardOverviewHandler : IGetAdminDashboardOverviewHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetAdminDashboardOverviewHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<AdminDashboardOverviewResponse> Handle(CancellationToken ct)
    {
        var totalUsers = await _adminQueries.GetTotalUsersCountAsync(ct);
        var activeVendors = await _adminQueries.GetActiveVendorsCountAsync(ct);
        var totalBookings = await _adminQueries.GetTotalBookingsCountAsync(ct);
        var platformRevenue = await _adminQueries.GetPlatformRevenueAsync(ct);

        // Calculate changes (simplified - compare with previous period)
        var previousMonth = DateTime.UtcNow.AddMonths(-1);
        // Note: For previous period calculations, we'd need additional query methods
        // For now, using simplified approach
        var usersChange = 0m;
        var vendorsChange = 0m;
        var bookingsChange = 0m;
        var revenueChange = 0m;

        var pendingListingsData = await _adminQueries.GetPendingListingsForApprovalAsync(5, ct);
        var pendingListings = pendingListingsData.Select(l => new PendingListingDto
        {
            Id = l.Id.ToString(),
            Name = l.Title,
            Location = $"{l.City}, {l.State}",
            VendorName = l.Vendor?.BusinessName ?? "",
            VendorAvatar = l.Vendor?.User?.UserProfile?.ProfileImageUrl ?? l.Vendor?.BusinessLogoUrl,
            Type = l.ListingType == ListingType.Hall ? "Hall" : "Service",
            Submitted = GetTimeAgo(l.CreatedAt),
            Status = "PENDING"
        }).ToList();

        var pendingCount = await _adminQueries.GetPendingListingsCountAsync(ct);
        var payoutsCount = await _adminQueries.GetPendingPayoutsCountAsync(ct);

        return new AdminDashboardOverviewResponse
        {
            Success = true,
            Data = new AdminDashboardOverviewData
            {
                TotalUsers = totalUsers,
                ActiveVendors = activeVendors,
                TotalBookings = totalBookings,
                PlatformRevenue = platformRevenue,
                UsersChange = (decimal)usersChange,
                VendorsChange = (decimal)vendorsChange,
                BookingsChange = (decimal)bookingsChange,
                RevenueChange = revenueChange,
                PendingListings = pendingListings,
                PendingCount = pendingCount,
                PayoutsCount = payoutsCount
            }
        };
    }

    private static string GetTimeAgo(DateTime dateTime)
    {
        var timeSpan = DateTime.UtcNow - dateTime;
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} minutes ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hours ago";
        return $"{(int)timeSpan.TotalDays} days ago";
    }
}

