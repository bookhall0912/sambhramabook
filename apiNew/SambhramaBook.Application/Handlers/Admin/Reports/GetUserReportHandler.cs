using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Reports;

public interface IGetUserReportHandler : IQueryHandler<GetUserReportRequest, GetUserReportResponse>;

public class GetUserReportHandler : IGetUserReportHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetUserReportHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetUserReportResponse> Handle(GetUserReportRequest request, CancellationToken ct)
    {
        var startDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
        var endDate = request.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var users = (await _adminQueries.GetUsersByDateRangeAsync(startDate, endDate, ct)).ToList();

        var totalUsers = users.Count;
        var newUsers = users.Count;
        var activeUsers = users.Count(u => u.IsActive);

        var breakdown = users
            .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
            .Select(g => new UserReportBreakdownDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                New = g.Count(),
                Active = g.Count(u => u.IsActive)
            })
            .OrderBy(d => d.Month)
            .ToList();

        return new GetUserReportResponse
        {
            Success = true,
            Data = new UserReportData
            {
                Period = new PeriodDto
                {
                    StartDate = startDate.ToString("yyyy-MM-dd"),
                    EndDate = endDate.ToString("yyyy-MM-dd")
                },
                TotalUsers = totalUsers,
                NewUsers = newUsers,
                ActiveUsers = activeUsers,
                Breakdown = breakdown
            }
        };
    }
}

