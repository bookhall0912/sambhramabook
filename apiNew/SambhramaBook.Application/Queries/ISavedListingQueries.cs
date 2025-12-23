using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Queries;

public interface ISavedListingQueries
{
    Task<(IEnumerable<SavedListing> SavedListings, int Total)> GetSavedListingsAsync(
        long userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}

