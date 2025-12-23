using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Reports;

public interface IGetBookingReportHandler : IQueryHandler<GetBookingReportRequest, GetBookingReportResponse>;

public class GetBookingReportHandler : IGetBookingReportHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetBookingReportHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetBookingReportResponse> Handle(GetBookingReportRequest request, CancellationToken ct)
    {
        var startDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
        var endDate = request.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var bookings = (await _adminQueries.GetBookingsByDateRangeAsync(startDate, endDate, ct)).ToList();

        var totalBookings = bookings.Count;
        var confirmed = bookings.Count(b => b.Status == BookingStatus.Confirmed);
        var cancelled = bookings.Count(b => b.Status == BookingStatus.Cancelled);

        var breakdown = bookings
            .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
            .Select(g => new BookingReportBreakdownDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                Total = g.Count(),
                Confirmed = g.Count(b => b.Status == BookingStatus.Confirmed),
                Cancelled = g.Count(b => b.Status == BookingStatus.Cancelled)
            })
            .OrderBy(d => d.Month)
            .ToList();

        return new GetBookingReportResponse
        {
            Success = true,
            Data = new BookingReportData
            {
                Period = new PeriodDto
                {
                    StartDate = startDate.ToString("yyyy-MM-dd"),
                    EndDate = endDate.ToString("yyyy-MM-dd")
                },
                TotalBookings = totalBookings,
                Confirmed = confirmed,
                Cancelled = cancelled,
                Breakdown = breakdown
            }
        };
    }
}

