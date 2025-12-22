using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.Repository;

public interface IListingRepository
{
    Task<Listing?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Listing?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Listing>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Listing>> SearchAsync(
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
        CancellationToken cancellationToken = default);
    Task<Listing> CreateAsync(Listing listing, CancellationToken cancellationToken = default);
    Task<Listing> UpdateAsync(Listing listing, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(
        string? location,
        decimal? minPrice,
        decimal? maxPrice,
        int? minCapacity,
        int? maxCapacity,
        ListingType? listingType,
        CancellationToken cancellationToken = default);
}

