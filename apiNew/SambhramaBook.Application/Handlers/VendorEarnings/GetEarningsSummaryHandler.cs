using SambhramaBook.Application.Models.VendorEarnings;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorEarnings;

public interface IGetEarningsSummaryHandler
{
    Task<GetEarningsSummaryResponse> Handle(long userId, GetEarningsSummaryRequest request, CancellationToken ct);
}

public class GetEarningsSummaryHandler : IGetEarningsSummaryHandler
{
    private readonly IVendorQueries _vendorQueries;

    public GetEarningsSummaryHandler(IVendorQueries vendorQueries)
    {
        _vendorQueries = vendorQueries;
    }

    public async Task<GetEarningsSummaryResponse> Handle(long userId, GetEarningsSummaryRequest request, CancellationToken ct)
    {
        var startDate = request.StartDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
        var endDate = request.EndDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var bookings = (await _vendorQueries.GetVendorBookingsForEarningsAsync(
            userId,
            startDate,
            endDate,
            ct)).ToList();

        var totalEarnings = bookings.Sum(b => b.TotalAmount - b.PlatformFee);

        var thisMonth = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1 - DateTime.UtcNow.Day);
        var lastMonth = thisMonth.AddMonths(-1);

        var thisMonthEarnings = bookings
            .Where(b => b.StartDate >= thisMonth && b.StartDate < thisMonth.AddMonths(1))
            .Sum(b => b.TotalAmount - b.PlatformFee);

        var lastMonthEarnings = bookings
            .Where(b => b.StartDate >= lastMonth && b.StartDate < thisMonth)
            .Sum(b => b.TotalAmount - b.PlatformFee);

        var pendingPayouts = await _vendorQueries.GetPendingPayoutsAmountAsync(userId, ct);

        return new GetEarningsSummaryResponse
        {
            Success = true,
            Data = new EarningsSummaryData
            {
                TotalEarnings = totalEarnings,
                ThisMonth = thisMonthEarnings,
                LastMonth = lastMonthEarnings,
                PendingPayouts = pendingPayouts,
                TotalTransactions = bookings.Count
            }
        };
    }
}

