using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Admin.Reports;

public interface IGetRevenueReportHandler : IQueryHandler<GetRevenueReportRequest, GetRevenueReportResponse>;

public class GetRevenueReportHandler : IGetRevenueReportHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetRevenueReportHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetRevenueReportResponse> Handle(GetRevenueReportRequest request, CancellationToken ct)
    {
        var startDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
        var endDate = request.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var bookings = (await _adminQueries.GetCompletedBookingsByDateRangeAsync(startDate, endDate, ct)).ToList();

        var totalRevenue = bookings.Sum(b => b.TotalAmount);
        var totalCommission = bookings.Sum(b => b.PlatformFee);

        var breakdown = bookings
            .GroupBy(b => new { b.StartDate.Year, b.StartDate.Month })
            .Select(g => new RevenueReportBreakdownDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                Revenue = g.Sum(b => b.TotalAmount),
                Commission = g.Sum(b => b.PlatformFee)
            })
            .OrderBy(d => d.Month)
            .ToList();

        return new GetRevenueReportResponse
        {
            Success = true,
            Data = new RevenueReportData
            {
                Period = new PeriodDto
                {
                    StartDate = startDate.ToString("yyyy-MM-dd"),
                    EndDate = endDate.ToString("yyyy-MM-dd")
                },
                TotalRevenue = totalRevenue,
                TotalCommission = totalCommission,
                Breakdown = breakdown
            }
        };
    }
}

