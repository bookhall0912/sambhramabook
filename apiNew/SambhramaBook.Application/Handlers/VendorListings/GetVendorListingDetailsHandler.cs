using SambhramaBook.Application.Models.VendorListings;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.VendorListings;

public interface IGetVendorListingDetailsHandler
{
    Task<VendorListingDetailDto?> Handle(long userId, long id, CancellationToken ct);
}

public class GetVendorListingDetailsHandler : IGetVendorListingDetailsHandler
{
    private readonly IVendorQueries _vendorQueries;

    public GetVendorListingDetailsHandler(IVendorQueries vendorQueries)
    {
        _vendorQueries = vendorQueries;
    }

    public async Task<VendorListingDetailDto?> Handle(long userId, long id, CancellationToken ct)
    {
        var listing = await _vendorQueries.GetVendorListingDetailsAsync(id, userId, ct);

        if (listing == null)
        {
            return null;
        }

        return new VendorListingDetailDto
        {
            Id = listing.Id.ToString(),
            Name = listing.Title,
            Description = listing.Description,
            AddressLine1 = listing.AddressLine1,
            City = listing.City,
            Pincode = listing.Pincode,
            FloatingCapacity = listing.CapacityMax,
            DiningCapacity = listing.CapacityMin,
            PricePerDay = listing.BasePrice,
            AdvanceAmount = 0, // AdvancePercentage doesn't exist on Listing
            CancellationPolicy = listing.CancellationPolicy,
            Amenities = listing.Amenities.Select(a => new VendorAmenityDto
            {
                Id = a.AmenityName,
                Name = a.AmenityName,
                Selected = a.IsAvailable
            }).ToList(),
            Images = listing.Images.OrderBy(img => img.DisplayOrder).Select(img => img.ImageUrl).ToList(),
            Status = listing.Status == Domain.Enums.ListingStatus.Approved ? "ACTIVE" :
                    listing.Status == Domain.Enums.ListingStatus.Draft ? "DRAFT" : "INACTIVE"
        };
    }
}

