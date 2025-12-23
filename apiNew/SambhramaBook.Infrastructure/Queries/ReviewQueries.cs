using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Queries;

public class ReviewQueries : IReviewQueries
{
    private readonly SambhramaBookDbContext _context;

    public ReviewQueries(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<Review> Reviews, int Total)> GetReviewsForListingAsync(
        long listingId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Reviews
            .Include(r => r.Customer)
            .Where(r => r.ListingId == listingId && r.IsPublished)
            .AsNoTracking();

        var total = await query.CountAsync(cancellationToken);
        var reviews = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (reviews, total);
    }
}

