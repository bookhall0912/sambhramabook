using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Admin.Vendors;

public interface IGetVendorDetailsHandler : IQueryHandler<long, AdminVendorDetailDto?>;

public class GetVendorDetailsHandler : IGetVendorDetailsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetVendorDetailsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<AdminVendorDetailDto?> Handle(long id, CancellationToken ct)
    {
        var vendor = await _adminQueries.GetVendorDetailsAsync(id, ct);
        if (vendor == null)
            return null;

        var listings = await _adminQueries.GetVendorListingsCountAsync(vendor.UserId, ct);
        var earnings = await _adminQueries.GetVendorEarningsAsync(vendor.UserId, ct);

        return new AdminVendorDetailDto
        {
            Id = vendor.Id.ToString(),
            Name = vendor.User?.Name ?? "",
            Email = vendor.User?.Email,
            Mobile = vendor.User?.Mobile,
            Avatar = vendor.User?.UserProfile?.ProfileImageUrl ?? vendor.BusinessLogoUrl,
            Status = vendor.User?.IsActive == true ? "ACTIVE" : "INACTIVE",
            JoinDate = vendor.CreatedAt.ToString("yyyy-MM-dd"),
            Listings = listings,
            Earnings = earnings,
            VerificationStatus = vendor.VerificationStatus.ToString().ToUpper(),
            BusinessName = vendor.BusinessName,
            BusinessType = vendor.BusinessType.ToString()
        };
    }
}

