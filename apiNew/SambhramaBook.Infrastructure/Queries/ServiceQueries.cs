using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.Queries;

public class ServiceQueries : IServiceQueries
{
    private readonly SambhramaBookDbContext _context;

    public ServiceQueries(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<Listing?> GetServiceDetailsWithIncludesAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Include(l => l.Vendor)
            .Include(l => l.Images)
            .Include(l => l.Reviews)
                .ThenInclude(r => r.Customer)
            .Include(l => l.ServicePackages)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<Listing> Listings, int Total)> GetServicesByTypeAsync(
        ListingType listingType,
        string? location,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Listings
            .Include(l => l.Images.Where(img => img.IsPrimary))
            .Where(l => l.ListingType == listingType &&
                       l.Status == ListingStatus.Approved &&
                       l.ApprovalStatus == ApprovalStatus.Approved)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(l => l.City.Contains(location) || l.State.Contains(location));
        }

        var total = await query.CountAsync(cancellationToken);
        var listings = await query
            .OrderByDescending(l => l.AverageRating)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (listings, total);
    }
}

