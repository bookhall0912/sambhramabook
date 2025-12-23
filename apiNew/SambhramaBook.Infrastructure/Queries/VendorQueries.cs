using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.Queries;

public class VendorQueries : IVendorQueries
{
    private readonly SambhramaBookDbContext _context;

    public VendorQueries(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<Listing?> GetVendorListingDetailsAsync(long listingId, long vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Include(l => l.Images)
            .Include(l => l.Amenities)
            .Where(l => l.Id == listingId && l.VendorId == vendorId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<ListingAvailability>> GetVendorAvailabilityAsync(
        long listingId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        return await _context.ListingAvailabilities
            .Where(la => la.ListingId == listingId &&
                        la.Date >= startDate &&
                        la.Date <= endDate)
            .OrderBy(la => la.Date)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetVendorBookingsForAvailabilityAsync(
        long listingId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.ListingId == listingId &&
                       b.Status != BookingStatus.Cancelled &&
                       b.StartDate <= endDate &&
                       b.EndDate >= startDate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Payout> Payouts, int Total)> GetPayoutHistoryAsync(
        long vendorId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Payouts
            .Where(p => p.VendorId == vendorId)
            .AsNoTracking();

        var total = await query.CountAsync(cancellationToken);
        var payouts = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (payouts, total);
    }

    public async Task<(IEnumerable<Payment> Transactions, int Total)> GetEarningsTransactionsAsync(
        long vendorId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Payments
            .Include(p => p.Booking)
            .Where(p => p.Booking != null && p.Booking.VendorId == vendorId && p.Status == PaymentStatus.Paid)
            .AsNoTracking();

        var total = await query.CountAsync(cancellationToken);
        var transactions = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (transactions, total);
    }

    public async Task<VendorProfile?> GetVendorSettingsAsync(long vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.VendorProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(vp => vp.Id == vendorId, cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetVendorBookingsForEarningsAsync(
        long vendorId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.VendorId == vendorId &&
                       b.PaymentStatus == PaymentStatus.Paid &&
                       b.StartDate >= startDate &&
                       b.StartDate <= endDate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetPendingPayoutsAmountAsync(long vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Where(p => p.VendorId == vendorId && p.Status == "PENDING")
            .AsNoTracking()
            .SumAsync(p => (decimal?)p.Amount, cancellationToken) ?? 0;
    }
}

