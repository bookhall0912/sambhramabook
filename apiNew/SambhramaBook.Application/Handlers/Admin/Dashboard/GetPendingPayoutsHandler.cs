using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Admin.Dashboard;

public interface IGetPendingPayoutsHandler : IQueryHandler<int, GetPendingPayoutsResponse>;

public class GetPendingPayoutsHandler : IGetPendingPayoutsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetPendingPayoutsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetPendingPayoutsResponse> Handle(int limit, CancellationToken ct)
    {
        var payouts = await _adminQueries.GetPendingPayoutsAsync(limit, ct);
        
        var payoutDtos = payouts.Select(p => new PendingPayoutDto
        {
            Id = p.Id.ToString(),
            VendorName = p.Vendor?.BusinessName ?? "",
            Amount = p.Amount,
            RequestDate = p.CreatedAt.ToString("yyyy-MM-dd")
        }).ToList();

        return new GetPendingPayoutsResponse
        {
            Success = true,
            Data = payoutDtos
        };
    }
}

