using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Analytics;

public interface IGetUserAnalyticsHandler : IQueryHandler<GetUserAnalyticsRequest, GetUserAnalyticsResponse>;

public class GetUserAnalyticsHandler : IGetUserAnalyticsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetUserAnalyticsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetUserAnalyticsResponse> Handle(GetUserAnalyticsRequest request, CancellationToken ct)
    {
        // Get all users for analytics (or use date range if provided)
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-10)); // Get all users
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var users = (await _adminQueries.GetUsersByDateRangeAsync(startDate, endDate, ct)).ToList();

        var breakdown = users
            .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
            .Select(g => new UserAnalyticsBreakdownDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                NewUsers = g.Count(),
                ActiveUsers = g.Count(u => u.IsActive) // Count users where IsActive is true
            })
            .OrderBy(d => d.Month)
            .ToList();

        return new GetUserAnalyticsResponse
        {
            Success = true,
            Data = new UserAnalyticsData
            {
                Period = request.Period,
                Data = breakdown
            }
        };
    }
}

