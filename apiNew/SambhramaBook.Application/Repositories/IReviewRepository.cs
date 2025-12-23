using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Review?> GetByBookingIdAsync(long bookingId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetByListingIdAsync(long listingId, CancellationToken cancellationToken = default);
    Task<Review> CreateAsync(Review review, CancellationToken cancellationToken = default);
    Task<Review> UpdateAsync(Review review, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}

