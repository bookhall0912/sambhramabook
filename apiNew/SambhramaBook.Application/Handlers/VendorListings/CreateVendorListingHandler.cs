using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.VendorListings;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.VendorListings;

public class CreateVendorListingHandler
{
    private readonly IListingRepository _listingRepository;
    private readonly IVendorProfileRepository _vendorProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateVendorListingHandler(
        IListingRepository listingRepository,
        IVendorProfileRepository vendorProfileRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _listingRepository = listingRepository;
        _vendorProfileRepository = vendorProfileRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CreateVendorListingResponse> HandleAsync(long userId, CreateVendorListingRequest request, CancellationToken cancellationToken = default)
    {
        var vendorProfile = await _vendorProfileRepository.GetByUserIdAsync(userId, cancellationToken);
        if (vendorProfile == null)
        {
            return new CreateVendorListingResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "Vendor profile not found" }
            };
        }

        // Generate slug from title
        var slug = request.Name.ToLower()
            .Replace(" ", "-")
            .Replace("'", "")
            .Replace("\"", "");
        slug = $"{slug}-{Guid.NewGuid().ToString("N")[..8]}";

        var listing = new Listing
        {
            VendorId = vendorProfile.Id,
            Title = request.Name,
            Slug = slug,
            Description = request.Description,
            ShortDescription = request.Description?.Length > 200 ? request.Description[..200] : request.Description,
            AddressLine1 = request.AddressLine1,
            City = request.City,
            State = vendorProfile.State,
            Pincode = request.Pincode,
            CapacityMin = request.DiningCapacity,
            CapacityMax = request.FloatingCapacity,
            BasePrice = request.PricePerDay,
            AreaSqft = !string.IsNullOrEmpty(request.Area) ? int.TryParse(request.Area, out var area) ? area : null : null,
            CancellationPolicy = request.CancellationPolicy,
            ListingType = ListingType.Hall, // Default to Hall, can be extended
            Status = request.Status == "ACTIVE" ? ListingStatus.Approved : ListingStatus.Draft,
            ApprovalStatus = ApprovalStatus.Pending,
            CreatedAt = _dateTimeProvider.GetUtcNow(),
            UpdatedAt = _dateTimeProvider.GetUtcNow()
        };

        // Add amenities
        if (request.Amenities != null)
        {
            foreach (var amenityId in request.Amenities)
            {
                listing.Amenities.Add(new ListingAmenity
                {
                    AmenityName = amenityId,
                    IsAvailable = true,
                    CreatedAt = _dateTimeProvider.GetUtcNow()
                });
            }
        }

        // Add images
        if (request.Images != null)
        {
            for (int i = 0; i < request.Images.Count; i++)
            {
                listing.Images.Add(new ListingImage
                {
                    ImageUrl = request.Images[i],
                    IsPrimary = i == 0,
                    DisplayOrder = i,
                    CreatedAt = _dateTimeProvider.GetUtcNow()
                });
            }
        }

        listing = await _listingRepository.CreateAsync(listing, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new CreateVendorListingResponse
        {
            Success = true,
            Data = new CreateVendorListingResponseData
            {
                Id = listing.Id.ToString(),
                Message = "Listing created successfully"
            }
        };
    }
}

