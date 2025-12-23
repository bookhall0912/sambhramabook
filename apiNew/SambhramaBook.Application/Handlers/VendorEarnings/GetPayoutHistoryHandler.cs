using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorEarnings;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.VendorEarnings;

public interface IGetPayoutHistoryHandler
{
    Task<GetPayoutHistoryResponse> Handle(long userId, GetPayoutHistoryRequest request, CancellationToken ct);
}

public class GetPayoutHistoryHandler : IGetPayoutHistoryHandler
{
    private readonly IVendorQueries _vendorQueries;

    public GetPayoutHistoryHandler(IVendorQueries vendorQueries)
    {
        _vendorQueries = vendorQueries;
    }

    public async Task<GetPayoutHistoryResponse> Handle(long userId, GetPayoutHistoryRequest request, CancellationToken ct)
    {
        var (payouts, total) = await _vendorQueries.GetPayoutHistoryAsync(
            userId,
            request.Page,
            request.PageSize,
            ct);

        var payoutDtos = payouts.Select(p => new PayoutHistoryDto
        {
            Id = p.Id.ToString(),
            Amount = p.Amount,
            Date = p.ProcessedAt?.ToString("yyyy-MM-dd") ?? p.CreatedAt.ToString("yyyy-MM-dd"),
            Status = p.Status.ToUpperInvariant() == "COMPLETED" ? "completed" :
                    p.Status.ToUpperInvariant() == "PENDING" ? "pending" : "failed",
            TransactionId = p.TransactionReference
        }).ToList();

        return new GetPayoutHistoryResponse
        {
            Success = true,
            Data = payoutDtos,
            Pagination = new PaginationInfo
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
            }
        };
    }
}

