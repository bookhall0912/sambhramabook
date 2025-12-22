using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class SavedListingRepository : ISavedListingRepository
{
    private readonly SambhramaBookDbContext _context;

    public SavedListingRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<SavedListing?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.SavedListings
            .Include(sl => sl.Listing)
                .ThenInclude(l => l.Images.Where(img => img.IsPrimary))
            .FirstOrDefaultAsync(sl => sl.Id == id, cancellationToken);
    }

    public async Task<SavedListing?> GetByUserAndListingAsync(long userId, long listingId, CancellationToken cancellationToken = default)
    {
        return await _context.SavedListings
            .FirstOrDefaultAsync(sl => sl.UserId == userId && sl.ListingId == listingId, cancellationToken);
    }

    public async Task<IEnumerable<SavedListing>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.SavedListings
            .Include(sl => sl.Listing)
                .ThenInclude(l => l.Images.Where(img => img.IsPrimary))
            .Where(sl => sl.UserId == userId)
            .OrderByDescending(sl => sl.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<SavedListing> CreateAsync(SavedListing savedListing, CancellationToken cancellationToken = default)
    {
        _context.SavedListings.Add(savedListing);
        await _context.SaveChangesAsync(cancellationToken);
        return savedListing;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var savedListing = await _context.SavedListings.FindAsync([id], cancellationToken);
        if (savedListing != null)
        {
            _context.SavedListings.Remove(savedListing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long userId, long listingId, CancellationToken cancellationToken = default)
    {
        return await _context.SavedListings
            .AnyAsync(sl => sl.UserId == userId && sl.ListingId == listingId, cancellationToken);
    }
}

