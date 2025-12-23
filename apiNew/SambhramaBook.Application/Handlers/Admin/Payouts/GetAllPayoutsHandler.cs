using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Admin.Payouts;

public interface IGetAllPayoutsHandler : IQueryHandler<GetAllPayoutsRequest, GetAllPayoutsResponse>;

public class GetAllPayoutsHandler : IGetAllPayoutsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetAllPayoutsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetAllPayoutsResponse> Handle(GetAllPayoutsRequest request, CancellationToken ct)
    {
        var (payouts, total) = await _adminQueries.GetAllPayoutsAsync(
            request.Status,
            request.Page,
            request.PageSize,
            ct);

        var payoutDtos = payouts.Select(p => new AdminPayoutDto
        {
            Id = p.Id.ToString(),
            VendorId = p.VendorId.ToString(),
            VendorName = p.Vendor?.BusinessName ?? "",
            Amount = p.Amount,
            RequestDate = p.CreatedAt.ToString("yyyy-MM-dd"),
            Status = p.Status.ToUpperInvariant() == "COMPLETED" ? "PROCESSED" :
                    p.Status.ToUpperInvariant() == "PENDING" ? "PENDING" : "FAILED",
            ProcessedDate = p.ProcessedAt?.ToString("yyyy-MM-dd")
        }).ToList();

        return new GetAllPayoutsResponse
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

