using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Reports;

public interface IGetVendorReportHandler : IQueryHandler<GetVendorReportRequest, GetVendorReportResponse>;

public class GetVendorReportHandler : IGetVendorReportHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetVendorReportHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetVendorReportResponse> Handle(GetVendorReportRequest request, CancellationToken ct)
    {
        var startDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
        var endDate = request.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var vendors = (await _adminQueries.GetVendorsByDateRangeAsync(startDate, endDate, ct)).ToList();

        var totalVendors = vendors.Count;
        var activeVendors = vendors.Count(vp => vp.User?.IsActive == true);
        var newVendors = vendors.Count;

        var breakdown = vendors
            .GroupBy(vp => new { vp.CreatedAt.Year, vp.CreatedAt.Month })
            .Select(g => new VendorReportBreakdownDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                New = g.Count(),
                Active = g.Count(vp => vp.User?.IsActive == true)
            })
            .OrderBy(d => d.Month)
            .ToList();

        return new GetVendorReportResponse
        {
            Success = true,
            Data = new VendorReportData
            {
                Period = new PeriodDto
                {
                    StartDate = startDate.ToString("yyyy-MM-dd"),
                    EndDate = endDate.ToString("yyyy-MM-dd")
                },
                TotalVendors = totalVendors,
                ActiveVendors = activeVendors,
                NewVendors = newVendors,
                Breakdown = breakdown
            }
        };
    }
}

