using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorListings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorListings;

public interface IGetVendorListingsHandler
{
    Task<GetVendorListingsResponse> Handle(long userId, GetVendorListingsRequest request, CancellationToken ct);
}

public class GetVendorListingsHandler : IGetVendorListingsHandler
{
    private readonly IListingRepository _listingRepository;

    public GetVendorListingsHandler(
        IListingRepository listingRepository)
    {
        _listingRepository = listingRepository;
    }

    public async Task<GetVendorListingsResponse> Handle(long userId, GetVendorListingsRequest request, CancellationToken ct)
    {
        var listings = await _listingRepository.GetByVendorIdAsync(userId, ct);
        var total = listings.Count();
        var pagedListings = listings
            .OrderByDescending(l => l.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var listingDtos = pagedListings.Select(l => new VendorListingDto
        {
            Id = l.Id.ToString(),
            Name = l.Title,
            Image = l.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
            Location = l.City,
            Status = l.Status == ListingStatus.Approved ? "ACTIVE" :
                    l.Status == ListingStatus.Draft ? "DRAFT" : "INACTIVE",
            Type = l.ListingType == ListingType.Hall ? "Hall" : "Service"
        }).ToList();

        return new GetVendorListingsResponse
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

