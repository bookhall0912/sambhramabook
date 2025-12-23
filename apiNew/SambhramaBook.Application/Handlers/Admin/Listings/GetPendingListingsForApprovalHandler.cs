using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Listings;

public interface IGetPendingListingsForApprovalHandler : IQueryHandler<GetPendingListingsForApprovalRequest, GetPendingListingsForApprovalResponse>;

public class GetPendingListingsForApprovalHandler : IGetPendingListingsForApprovalHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetPendingListingsForApprovalHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetPendingListingsForApprovalResponse> Handle(GetPendingListingsForApprovalRequest request, CancellationToken ct)
    {
        var (listings, total) = await _adminQueries.GetPendingListingsForApprovalAsync(
            request.Page,
            request.PageSize,
            ct);

        var listingDtos = listings.Select(l => new AdminPendingListingDto
        {
            Id = l.Id.ToString(),
            Name = l.Title,
            Location = $"{l.City}, {l.State}",
            VendorName = l.Vendor?.BusinessName ?? "",
            VendorAvatar = l.Vendor?.User?.UserProfile?.ProfileImageUrl ?? l.Vendor?.BusinessLogoUrl,
            Type = l.ListingType == ListingType.Hall ? "Hall" : "Service",
            Submitted = l.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            Status = l.ApprovalStatus == ApprovalStatus.Pending ? "PENDING" : "NEEDS_CHANGES"
        }).ToList();

        return new GetPendingListingsForApprovalResponse
        {
            Success = true,
            Data = listingDtos,
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

