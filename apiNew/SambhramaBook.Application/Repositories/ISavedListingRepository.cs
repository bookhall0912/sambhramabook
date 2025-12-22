using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface ISavedListingRepository
{
    Task<SavedListing?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<SavedListing?> GetByUserAndListingAsync(long userId, long listingId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SavedListing>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<SavedListing> CreateAsync(SavedListing savedListing, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long userId, long listingId, CancellationToken cancellationToken = default);
}

