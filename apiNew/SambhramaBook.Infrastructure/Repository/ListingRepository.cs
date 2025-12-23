using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.Repository;

public class ListingRepository : IListingRepository
{
    private readonly SambhramaBookDbContext _context;

    public ListingRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<Listing?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Include(l => l.Vendor)
            .Include(l => l.Images)
            .Include(l => l.Amenities)
            .Include(l => l.ServicePackages)
            .Include(l => l.Reviews.Where(r => r.IsPublished))
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Listing?> GetByIdWithIncludesAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Include(l => l.Vendor)
            .Include(l => l.Images)
            .Include(l => l.Amenities)
            .Include(l => l.ServicePackages)
            .Include(l => l.Reviews)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Listing?> GetByVendorIdAndIdAsync(long vendorId, long id, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .FirstOrDefaultAsync(l => l.Id == id && l.VendorId == vendorId, cancellationToken);
    }

    public async Task<Listing?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Include(l => l.Vendor)
            .Include(l => l.Images)
            .Include(l => l.Amenities)
            .Include(l => l.ServicePackages)
            .Include(l => l.Reviews.Where(r => r.IsPublished))
            .FirstOrDefaultAsync(l => l.Slug == slug, cancellationToken);
    }

    public async Task<IEnumerable<Listing>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Include(l => l.Images.Where(img => img.IsPrimary))
            .Where(l => l.VendorId == vendorId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Listing>> SearchAsync(
        string? location,
        decimal? minPrice,
        decimal? maxPrice,
        int? minCapacity,
        int? maxCapacity,
        string? amenities,
        DateOnly? date,
        int? days,
        int? guests,
        ListingType? listingType,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Listings
            .Include(l => l.Images.Where(img => img.IsPrimary))
            .Include(l => l.Vendor)
            .Where(l => l.Status == ListingStatus.Approved && 
                       l.ApprovalStatus == ApprovalStatus.Approved);

        if (listingType.HasValue)
        {
            query = query.Where(l => l.ListingType == listingType.Value);
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(l => 
                l.City.Contains(location) || 
                l.State.Contains(location) ||
                l.AddressLine1.Contains(location));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(l => l.BasePrice >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(l => l.BasePrice <= maxPrice.Value);
        }

        if (minCapacity.HasValue)
        {
            query = query.Where(l => l.CapacityMax >= minCapacity.Value);
        }

        if (maxCapacity.HasValue)
        {
            query = query.Where(l => l.CapacityMin <= maxCapacity.Value);
        }

        if (!string.IsNullOrWhiteSpace(amenities))
        {
            var amenityList = amenities.Split(',', StringSplitOptions.RemoveEmptyEntries);
            query = query.Where(l => l.Amenities.Any(a => 
                amenityList.Contains(a.AmenityName) && a.IsAvailable));
        }

        // Availability check
        if (date.HasValue && days.HasValue)
        {
            var endDate = date.Value.AddDays(days.Value - 1);
            var bookedListingIds = await _context.Bookings
                .Where(b => b.Status != BookingStatus.Cancelled &&
                           b.StartDate <= endDate &&
                           b.EndDate >= date.Value)
                .Select(b => b.ListingId)
                .Distinct()
                .ToListAsync(cancellationToken);

            query = query.Where(l => !bookedListingIds.Contains(l.Id));
        }

        return await query
            .OrderByDescending(l => l.AverageRating)
            .ThenByDescending(l => l.ViewCount)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Listing> CreateAsync(Listing listing, CancellationToken cancellationToken = default)
    {
        _context.Listings.Add(listing);
        await _context.SaveChangesAsync(cancellationToken);
        return listing;
    }

    public async Task<Listing> UpdateAsync(Listing listing, CancellationToken cancellationToken = default)
    {
        listing.UpdatedAt = DateTime.UtcNow;
        _context.Listings.Update(listing);
        await _context.SaveChangesAsync(cancellationToken);
        return listing;
    }

    public async Task<int> GetTotalCountAsync(
        string? location,
        decimal? minPrice,
        decimal? maxPrice,
        int? minCapacity,
        int? maxCapacity,
        ListingType? listingType,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Listings
            .Where(l => l.Status == ListingStatus.Approved && 
                       l.ApprovalStatus == ApprovalStatus.Approved);

        if (listingType.HasValue)
        {
            query = query.Where(l => l.ListingType == listingType.Value);
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(l => 
                l.City.Contains(location) || 
                l.State.Contains(location) ||
                l.AddressLine1.Contains(location));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(l => l.BasePrice >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(l => l.BasePrice <= maxPrice.Value);
        }

        if (minCapacity.HasValue)
        {
            query = query.Where(l => l.CapacityMax >= minCapacity.Value);
        }

        if (maxCapacity.HasValue)
        {
            query = query.Where(l => l.CapacityMin <= maxCapacity.Value);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var listing = await _context.Listings.FindAsync([id], cancellationToken);
        if (listing != null)
        {
            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

