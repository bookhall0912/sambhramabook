using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class ListingAvailabilityRepository : IListingAvailabilityRepository
{
    private readonly SambhramaBookDbContext _context;

    public ListingAvailabilityRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<ListingAvailability?> GetByListingIdAndDateAsync(long listingId, DateOnly date, CancellationToken cancellationToken = default)
    {
        return await _context.ListingAvailabilities
            .FirstOrDefaultAsync(la => la.ListingId == listingId && la.Date == date, cancellationToken);
    }

    public async Task<IEnumerable<ListingAvailability>> GetByListingIdAsync(long listingId, CancellationToken cancellationToken = default)
    {
        return await _context.ListingAvailabilities
            .Where(la => la.ListingId == listingId)
            .ToListAsync(cancellationToken);
    }

    public async Task<ListingAvailability> CreateAsync(ListingAvailability availability, CancellationToken cancellationToken = default)
    {
        _context.ListingAvailabilities.Add(availability);
        await _context.SaveChangesAsync(cancellationToken);
        return availability;
    }

    public async Task<ListingAvailability> UpdateAsync(ListingAvailability availability, CancellationToken cancellationToken = default)
    {
        availability.UpdatedAt = DateTime.UtcNow;
        _context.ListingAvailabilities.Update(availability);
        await _context.SaveChangesAsync(cancellationToken);
        return availability;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var availability = await _context.ListingAvailabilities.FindAsync([id], cancellationToken);
        if (availability != null)
        {
            _context.ListingAvailabilities.Remove(availability);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteByListingIdAndDateAsync(long listingId, DateOnly date, CancellationToken cancellationToken = default)
    {
        var availability = await GetByListingIdAndDateAsync(listingId, date, cancellationToken);
        if (availability != null)
        {
            await DeleteAsync(availability.Id, cancellationToken);
        }
    }
}

