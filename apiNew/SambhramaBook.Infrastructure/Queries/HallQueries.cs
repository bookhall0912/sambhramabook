using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Models.Hall;
using SambhramaBook.Application.Queries;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.Queries;

public class HallQueries : IHallQueries
{
    private readonly IListingRepository _listingRepository;
    private readonly SambhramaBookDbContext _context;
    private readonly IMapper _mapper;

    public HallQueries(
        IListingRepository listingRepository,
        SambhramaBookDbContext context,
        IMapper mapper)
    {
        _listingRepository = listingRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HallListItemDto>> GetPopularHallsAsync(int limit, CancellationToken cancellationToken = default)
    {
        var listings = await _context.Listings
            .Include(l => l.Images.Where(img => img.IsPrimary))
            .Include(l => l.Amenities)
            .Include(l => l.Vendor)
            .Where(l => l.ListingType == ListingType.Hall &&
                       l.Status == ListingStatus.Approved &&
                       l.ApprovalStatus == ApprovalStatus.Approved)
            .OrderByDescending(l => l.AverageRating)
            .ThenByDescending(l => l.BookingCount)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return listings.Select(MapToListItem);
    }

    public async Task<HallSearchResponseDto> SearchHallsAsync(HallSearchRequestDto request, CancellationToken cancellationToken = default)
    {
        var listings = await _listingRepository.SearchAsync(
            request.Location,
            request.MinPrice,
            request.MaxPrice,
            request.MinCapacity,
            request.MaxCapacity,
            request.Amenities,
            request.Date,
            request.Days,
            request.Guests,
            ListingType.Hall,
            request.Page,
            request.PageSize,
            cancellationToken);

        var total = await _listingRepository.GetTotalCountAsync(
            request.Location,
            request.MinPrice,
            request.MaxPrice,
            request.MinCapacity,
            request.MaxCapacity,
            ListingType.Hall,
            cancellationToken);

        return new HallSearchResponseDto
        {
            Halls = listings.Select(MapToListItem).ToList(),
            Total = total,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
        };
    }

    public async Task<HallDetailDto?> GetHallByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(id, cancellationToken);
        if (listing == null || listing.ListingType != ListingType.Hall)
            return null;

        return MapToDetail(listing);
    }

    public async Task<HallDetailDto?> GetHallBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetBySlugAsync(slug, cancellationToken);
        if (listing == null || listing.ListingType != ListingType.Hall)
            return null;

        return MapToDetail(listing);
    }

    public async Task<HallAvailabilityResponseDto> GetHallAvailabilityAsync(long id, string month, int year, CancellationToken cancellationToken = default)
    {
        var monthNumber = DateTime.ParseExact(month, "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month;
        var startDate = new DateOnly(year, monthNumber, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var availability = await _context.ListingAvailabilities
            .Where(la => la.ListingId == id &&
                        la.Date >= startDate &&
                        la.Date <= endDate)
            .OrderBy(la => la.Date)
            .ToListAsync();

        var bookings = await _context.Bookings
            .Where(b => b.ListingId == id &&
                       b.Status != BookingStatus.Cancelled &&
                       b.StartDate <= endDate &&
                       b.EndDate >= startDate)
            .ToListAsync();

        var days = new List<AvailabilityDayDto>();
        var currentDate = startDate;

        while (currentDate <= endDate)
        {
            var dayAvailability = availability.FirstOrDefault(a => a.Date == currentDate);
            var isBooked = bookings.Any(b => 
                currentDate >= b.StartDate && currentDate <= b.EndDate);

            days.Add(new AvailabilityDayDto
            {
                Day = currentDate.Day,
                Status = isBooked ? "booked" : 
                        dayAvailability?.Status == Domain.Enums.AvailabilityStatus.Blocked ? "unavailable" :
                        dayAvailability?.Status == Domain.Enums.AvailabilityStatus.Maintenance ? "unavailable" :
                        "available",
                Price = dayAvailability?.PriceOverride
            });

            currentDate = currentDate.AddDays(1);
        }

        return new HallAvailabilityResponseDto
        {
            Month = month,
            Year = year,
            Days = days
        };
    }

    public async Task<IEnumerable<HallListItemDto>> GetSimilarHallsAsync(long hallId, int limit, CancellationToken cancellationToken = default)
    {
        // Get the reference hall
        var referenceHall = await _context.Listings
            .Include(l => l.Images.Where(img => img.IsPrimary))
            .FirstOrDefaultAsync(l => l.Id == hallId && l.ListingType == ListingType.Hall, cancellationToken);

        if (referenceHall == null)
            return Enumerable.Empty<HallListItemDto>();

        // Find similar halls based on:
        // 1. Same city (highest priority)
        // 2. Similar capacity range (within 20% of reference)
        // 3. Similar price range (within 30% of reference)
        // 4. High rating
        var capacityRange = referenceHall.CapacityMax.HasValue && referenceHall.CapacityMin.HasValue
            ? (referenceHall.CapacityMax.Value - referenceHall.CapacityMin.Value) * 0.2m
            : 100;

        var priceRange = referenceHall.BasePrice * 0.3m;

        var similarHalls = await _context.Listings
            .Include(l => l.Images.Where(img => img.IsPrimary))
            .Include(l => l.Amenities)
            .Include(l => l.Vendor)
            .Where(l => l.Id != hallId &&
                       l.ListingType == ListingType.Hall &&
                       l.Status == ListingStatus.Approved &&
                       l.ApprovalStatus == ApprovalStatus.Approved &&
                       l.City == referenceHall.City && // Same city
                       // Similar capacity (if both have capacity, otherwise include all)
                       (!referenceHall.CapacityMax.HasValue || !referenceHall.CapacityMin.HasValue || 
                        !l.CapacityMax.HasValue || !l.CapacityMin.HasValue ||
                        (l.CapacityMax >= referenceHall.CapacityMin - capacityRange &&
                         l.CapacityMin <= referenceHall.CapacityMax + capacityRange)))
            .OrderByDescending(l => 
                // Prioritize: similar price, high rating, booking count
                (l.BasePrice >= referenceHall.BasePrice - priceRange && 
                 l.BasePrice <= referenceHall.BasePrice + priceRange ? 100 : 0) +
                l.AverageRating * 20 +
                l.BookingCount)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return similarHalls.Select(MapToListItem);
    }

    private HallListItemDto MapToListItem(Domain.Entities.Listing listing)
    {
        return new HallListItemDto
        {
            Id = listing.Id,
            Title = listing.Title,
            Location = $"{listing.AddressLine1}, {listing.City}",
            City = listing.City,
            State = listing.State,
            AverageRating = listing.AverageRating,
            ReviewCount = listing.TotalReviews,
            CapacityMin = listing.CapacityMin,
            CapacityMax = listing.CapacityMax,
            BasePrice = listing.BasePrice,
            ImageUrl = listing.Images.FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
            Amenities = listing.Amenities.Where(a => a.IsAvailable).Select(a => a.AmenityName).ToList(),
            Description = listing.ShortDescription,
            Parking = listing.ParkingCapacity.HasValue ? $"{listing.ParkingCapacity}+ Cars Parking" : null,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude
        };
    }

    private HallDetailDto MapToDetail(Domain.Entities.Listing listing)
    {
        var item = MapToListItem(listing);
        
        return new HallDetailDto
        {
            Id = item.Id,
            Title = item.Title,
            Slug = listing.Slug,
            Location = item.Location,
            City = item.City,
            State = item.State,
            AverageRating = item.AverageRating,
            ReviewCount = item.ReviewCount,
            CapacityMin = item.CapacityMin,
            CapacityMax = item.CapacityMax,
            BasePrice = item.BasePrice,
            ImageUrl = item.ImageUrl,
            Images = listing.Images.OrderBy(img => img.DisplayOrder).Select(img => img.ImageUrl).ToList(),
            Amenities = item.Amenities,
            FullAmenities = listing.Amenities.Select(a => new AmenityDto
            {
                Name = a.AmenityName,
                Category = a.AmenityCategory,
                IconUrl = a.IconUrl
            }).ToList(),
            Description = item.Description,
            FullDescription = listing.Description,
            Parking = item.Parking,
            Latitude = item.Latitude,
            Longitude = item.Longitude,
            CancellationPolicy = listing.CancellationPolicy,
            AreaSqft = listing.AreaSqft,
            ParkingCapacity = listing.ParkingCapacity,
            Vendor = new VendorInfoDto
            {
                Id = listing.Vendor.Id,
                BusinessName = listing.Vendor.BusinessName,
                BusinessLogoUrl = listing.Vendor.BusinessLogoUrl,
                AverageRating = listing.Vendor.AverageRating,
                TotalReviews = listing.Vendor.TotalReviews
            },
            Reviews = listing.Reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                UserName = r.Customer.Name,
                Rating = r.Rating,
                Title = r.Title,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }
}

