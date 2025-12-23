using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Analytics;

public interface IGetBookingAnalyticsHandler : IQueryHandler<GetBookingAnalyticsRequest, GetBookingAnalyticsResponse>;

public class GetBookingAnalyticsHandler : IGetBookingAnalyticsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetBookingAnalyticsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetBookingAnalyticsResponse> Handle(GetBookingAnalyticsRequest request, CancellationToken ct)
    {
        // Get all bookings for analytics
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-10)); // Get all bookings
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var bookings = (await _adminQueries.GetBookingsByDateRangeAsync(startDate, endDate, ct)).ToList();

        var breakdown = bookings
            .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
            .Select(g => new BookingAnalyticsBreakdownDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                TotalBookings = g.Count(),
                Confirmed = g.Count(b => b.Status == BookingStatus.Confirmed),
                Cancelled = g.Count(b => b.Status == BookingStatus.Cancelled)
            })
            .OrderBy(d => d.Month)
            .ToList();

        return new GetBookingAnalyticsResponse
        {
            Success = true,
            Data = new BookingAnalyticsData
            {
                Period = request.Period,
                Data = breakdown
            }
        };
    }
}

