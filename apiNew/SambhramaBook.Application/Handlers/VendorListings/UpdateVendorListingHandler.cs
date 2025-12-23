using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorListings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorListings;

public class UpdateVendorListingHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateVendorListingHandler(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UpdateVendorListingResponse> HandleAsync(long userId, long listingId, CreateVendorListingRequest request, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithIncludesAsync(listingId, cancellationToken);
        
        if (listing == null || listing.VendorId != userId)

        if (listing == null)
        {
            return new UpdateVendorListingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Listing not found" }
            };
        }

        listing.Title = request.Name;
        listing.Description = request.Description;
        listing.ShortDescription = request.Description?.Length > 200 ? request.Description[..200] : request.Description;
        listing.AddressLine1 = request.AddressLine1;
        listing.City = request.City;
        listing.Pincode = request.Pincode;
        listing.CapacityMin = request.DiningCapacity;
        listing.CapacityMax = request.FloatingCapacity;
        listing.BasePrice = request.PricePerDay;
        listing.AreaSqft = !string.IsNullOrEmpty(request.Area) ? int.TryParse(request.Area, out var area) ? area : null : null;
        listing.CancellationPolicy = request.CancellationPolicy;
        listing.Status = request.Status == "ACTIVE" ? ListingStatus.Approved : ListingStatus.Draft;
        listing.UpdatedAt = _dateTimeProvider.GetUtcNow();

        // Update amenities
        if (request.Amenities != null)
        {
            listing.Amenities.Clear();
            foreach (var amenityId in request.Amenities)
            {
                listing.Amenities.Add(new Domain.Entities.ListingAmenity
                {
                    AmenityName = amenityId,
                    IsAvailable = true,
                    CreatedAt = _dateTimeProvider.GetUtcNow()
                });
            }
        }

        // Update images (if provided)
        if (request.Images != null && request.Images.Any())
        {
            listing.Images.Clear();
            for (int i = 0; i < request.Images.Count; i++)
            {
                listing.Images.Add(new Domain.Entities.ListingImage
                {
                    ImageUrl = request.Images[i],
                    IsPrimary = i == 0,
                    DisplayOrder = i,
                    CreatedAt = _dateTimeProvider.GetUtcNow()
                });
            }
        }

        await _listingRepository.UpdateAsync(listing, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new UpdateVendorListingResponse
        {
            Success = true,
            Data = new UpdateVendorListingResponseData
            {
                Id = listing.Id.ToString(),
                Message = "Listing updated successfully"
            }
        };
    }
}

