using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class BookingRepository : IBookingRepository
{
    private readonly SambhramaBookDbContext _context;

    public BookingRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<Booking?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Vendor)
            .Include(b => b.Listing)
                .ThenInclude(l => l.Images.Where(img => img.IsPrimary))
            .Include(b => b.ServicePackage)
            .Include(b => b.Guests)
            .Include(b => b.Payments)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<Booking?> GetByReferenceAsync(string reference, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Vendor)
            .Include(b => b.Listing)
                .ThenInclude(l => l.Images.Where(img => img.IsPrimary))
            .Include(b => b.ServicePackage)
            .Include(b => b.Guests)
            .Include(b => b.Payments)
            .FirstOrDefaultAsync(b => b.BookingReference == reference, cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetByCustomerIdAsync(long customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Listing)
                .ThenInclude(l => l.Images.Where(img => img.IsPrimary))
            .Include(b => b.Vendor)
            .Where(b => b.CustomerId == customerId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Listing)
                .ThenInclude(l => l.Images.Where(img => img.IsPrimary))
            .Where(b => b.VendorId == vendorId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetByListingIdAsync(long listingId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.ListingId == listingId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Booking> CreateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(booking.BookingReference))
        {
            booking.BookingReference = await GenerateBookingReferenceAsync(cancellationToken);
        }

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);
        return booking;
    }

    public async Task<Booking> UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        booking.UpdatedAt = DateTime.UtcNow;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync(cancellationToken);
        return booking;
    }

    public async Task<string> GenerateBookingReferenceAsync(CancellationToken cancellationToken = default)
    {
        var sequenceValue = await _context.Database
            .SqlQueryRaw<int>($"SELECT nextval('{SambhramaBookDbContext.BookingReferenceSequenceName}')")
            .FirstOrDefaultAsync(cancellationToken);

        var datePrefix = DateTime.UtcNow.ToString("yyMMdd");
        var randomSuffix = Guid.NewGuid().ToString("N")[..4].ToUpper();
        
        return $"SB-{datePrefix}-{randomSuffix}";
    }
}

