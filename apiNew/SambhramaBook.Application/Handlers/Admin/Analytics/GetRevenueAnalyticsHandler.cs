using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Analytics;

public interface IGetRevenueAnalyticsHandler : IQueryHandler<GetRevenueAnalyticsRequest, GetRevenueAnalyticsResponse>;

public class GetRevenueAnalyticsHandler : IGetRevenueAnalyticsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetRevenueAnalyticsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetRevenueAnalyticsResponse> Handle(GetRevenueAnalyticsRequest request, CancellationToken ct)
    {
        var startDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
        var endDate = request.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var bookings = (await _adminQueries.GetCompletedBookingsByDateRangeAsync(startDate, endDate, ct)).ToList();

        var breakdown = bookings
            .GroupBy(b => new { b.StartDate.Year, b.StartDate.Month })
            .Select(g => new RevenueBreakdownDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                Revenue = g.Sum(b => b.TotalAmount),
                Bookings = g.Count()
            })
            .OrderBy(d => d.Month)
            .ToList();

        return new GetRevenueAnalyticsResponse
        {
            Success = true,
            Data = new RevenueAnalyticsData
            {
                Period = request.Period,
                Data = breakdown
            }
        };
    }
}

