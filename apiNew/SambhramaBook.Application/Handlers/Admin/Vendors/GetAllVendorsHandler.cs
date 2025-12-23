using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Admin.Vendors;

public interface IGetAllVendorsHandler : IQueryHandler<GetAllVendorsRequest, GetAllVendorsResponse>;

public class GetAllVendorsHandler : IGetAllVendorsHandler
{
    private readonly IAdminQueries _adminQueries;

    public GetAllVendorsHandler(IAdminQueries adminQueries)
    {
        _adminQueries = adminQueries;
    }

    public async Task<GetAllVendorsResponse> Handle(GetAllVendorsRequest request, CancellationToken ct)
    {
        var (vendors, total) = await _adminQueries.GetAllVendorsAsync(
            null, // Search parameter not in request model
            request.Status,
            request.Page,
            request.PageSize,
            ct);

        var vendorDtos = new List<AdminVendorDto>();
        foreach (var vendor in vendors)
        {
            var listingsCount = await _adminQueries.GetVendorListingsCountAsync(vendor.UserId, ct);
            var earnings = await _adminQueries.GetVendorEarningsAsync(vendor.UserId, ct);
            
            vendorDtos.Add(new AdminVendorDto
            {
                Id = vendor.Id.ToString(),
                Name = vendor.User?.Name ?? "",
                Email = vendor.User?.Email,
                Mobile = vendor.User?.Mobile,
                Avatar = vendor.User?.UserProfile?.ProfileImageUrl ?? vendor.BusinessLogoUrl,
                Status = vendor.User?.IsActive == true ? "ACTIVE" : "INACTIVE",
                JoinDate = vendor.CreatedAt.ToString("yyyy-MM-dd"),
                Listings = listingsCount,
                Earnings = earnings,
                VerificationStatus = vendor.VerificationStatus.ToString().ToUpper()
            });
        }

        return new GetAllVendorsResponse
        {
            Success = true,
            Data = vendorDtos,
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

