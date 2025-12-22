using Microsoft.EntityFrameworkCore;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly SambhramaBookDbContext _context;

    public ReviewRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<Review?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Include(r => r.Customer)
            .Include(r => r.Vendor)
            .Include(r => r.Listing)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Review?> GetByBookingIdAsync(long bookingId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Include(r => r.Customer)
            .Include(r => r.Vendor)
            .Include(r => r.Listing)
            .FirstOrDefaultAsync(r => r.BookingId == bookingId, cancellationToken);
    }

    public async Task<IEnumerable<Review>> GetByListingIdAsync(long listingId, CancellationToken cancellationToken = default)
    {
        return await _context.Reviews
            .Include(r => r.Customer)
            .Where(r => r.ListingId == listingId && r.IsPublished)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Review> CreateAsync(Review review, CancellationToken cancellationToken = default)
    {
        _context.Reviews.Add(review);
        await _context.SaveChangesAsync(cancellationToken);
        return review;
    }

    public async Task<Review> UpdateAsync(Review review, CancellationToken cancellationToken = default)
    {
        review.UpdatedAt = DateTime.UtcNow;
        _context.Reviews.Update(review);
        await _context.SaveChangesAsync(cancellationToken);
        return review;
    }
}

