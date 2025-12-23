using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Dashboard;

public interface IGetPendingListingsHandler : IQueryHandler<int, GetPendingListingsResponse>;

public class GetPendingListingsHandler : IGetPendingListingsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetPendingListingsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetPendingListingsResponse> Handle(int limit, CancellationToken ct)
    {
        var listings = await _adminQueries.GetPendingListingsForApprovalAsync(limit, ct);
        
        var listingDtos = listings.Select(l => new PendingListingDto
        {
            Id = l.Id.ToString(),
            Name = l.Title,
            Location = $"{l.City}, {l.State}",
            VendorName = l.Vendor?.BusinessName ?? "",
            VendorAvatar = l.Vendor?.User?.UserProfile?.ProfileImageUrl ?? l.Vendor?.BusinessLogoUrl,
            Type = l.ListingType == ListingType.Hall ? "Hall" : "Service",
            Submitted = GetTimeAgo(l.CreatedAt),
            Status = "PENDING"
        }).ToList();

        return new GetPendingListingsResponse
        {
            Success = true,
            Data = listingDtos
        };
    }

    private static string GetTimeAgo(DateTime dateTime)
    {
        var timeSpan = DateTime.UtcNow - dateTime;
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} minutes ago";
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hours ago";
        return $"{(int)timeSpan.TotalDays} days ago";
    }
}

