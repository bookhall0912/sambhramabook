using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface IListingAvailabilityRepository
{
    Task<ListingAvailability?> GetByListingIdAndDateAsync(long listingId, DateOnly date, CancellationToken cancellationToken = default);
    Task<IEnumerable<ListingAvailability>> GetByListingIdAsync(long listingId, CancellationToken cancellationToken = default);
    Task<ListingAvailability> CreateAsync(ListingAvailability availability, CancellationToken cancellationToken = default);
    Task<ListingAvailability> UpdateAsync(ListingAvailability availability, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task DeleteByListingIdAndDateAsync(long listingId, DateOnly date, CancellationToken cancellationToken = default);
}

