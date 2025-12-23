using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Admin.Analytics;

public interface IGetAnalyticsSummaryHandler : IQueryHandler<GetAnalyticsSummaryRequest, GetAnalyticsSummaryResponse>;

public class GetAnalyticsSummaryHandler : IGetAnalyticsSummaryHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetAnalyticsSummaryHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetAnalyticsSummaryResponse> Handle(GetAnalyticsSummaryRequest request, CancellationToken ct)
    {
        var startDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
        var endDate = request.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var totalRevenue = await _adminQueries.GetTotalRevenueAsync(startDate, endDate, ct);
        var totalBookings = await _adminQueries.GetTotalBookingsCountAsync(startDate, endDate, ct);
        var totalUsers = await _adminQueries.GetTotalUsersCountAsync(startDate, endDate, ct);
        var totalVendors = await _adminQueries.GetTotalVendorsCountAsync(startDate, endDate, ct);

        var averageBookingValue = totalBookings > 0 ? totalRevenue / totalBookings : 0;

        return new GetAnalyticsSummaryResponse
        {
            Success = true,
            Data = new AnalyticsSummaryData
            {
                TotalRevenue = totalRevenue,
                TotalBookings = totalBookings,
                TotalUsers = totalUsers,
                TotalVendors = totalVendors,
                AverageBookingValue = averageBookingValue
            }
        };
    }
}

