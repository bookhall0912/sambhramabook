using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Queries;

public class SavedListingQueries : ISavedListingQueries
{
    private readonly SambhramaBookDbContext _context;

    public SavedListingQueries(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<SavedListing> SavedListings, int Total)> GetSavedListingsAsync(
        long userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.SavedListings
            .Include(sl => sl.Listing)
                .ThenInclude(l => l.Images.Where(img => img.IsPrimary))
            .Where(sl => sl.UserId == userId)
            .AsNoTracking();

        var total = await query.CountAsync(cancellationToken);
        var savedListings = await query
            .OrderByDescending(sl => sl.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (savedListings, total);
    }
}

