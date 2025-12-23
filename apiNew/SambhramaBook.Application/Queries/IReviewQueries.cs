using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Queries;

public interface IReviewQueries
{
    Task<(IEnumerable<Review> Reviews, int Total)> GetReviewsForListingAsync(
        long listingId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}

