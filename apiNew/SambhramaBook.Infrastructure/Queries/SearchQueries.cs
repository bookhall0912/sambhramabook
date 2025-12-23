using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.Queries;

public class SearchQueries : ISearchQueries
{
    private readonly SambhramaBookDbContext _context;

    public SearchQueries(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Listing>> SearchHallListingsAsync(string searchTerm, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Include(l => l.Images.Where(img => img.IsPrimary))
            .Where(l => l.ListingType == ListingType.Hall &&
                       l.Status == ListingStatus.Approved &&
                       l.ApprovalStatus == ApprovalStatus.Approved &&
                       (l.Title.ToLower().Contains(searchTerm) ||
                        l.City.ToLower().Contains(searchTerm) ||
                        l.State.ToLower().Contains(searchTerm)))
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Listing>> SearchServiceListingsAsync(string searchTerm, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Include(l => l.Images.Where(img => img.IsPrimary))
            .Where(l => l.ListingType != ListingType.Hall &&
                       l.Status == ListingStatus.Approved &&
                       l.ApprovalStatus == ApprovalStatus.Approved &&
                       (l.Title.ToLower().Contains(searchTerm) ||
                        l.City.ToLower().Contains(searchTerm)))
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VendorProfile>> SearchVendorProfilesAsync(string searchTerm, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.VendorProfiles
            .Include(vp => vp.User)
            .Where(vp => vp.BusinessName.ToLower().Contains(searchTerm) ||
                        vp.City.ToLower().Contains(searchTerm))
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

