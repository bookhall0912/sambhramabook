using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Infrastructure.Queries;

public class AdminQueries : IAdminQueries
{
    private readonly SambhramaBookDbContext _context;

    public AdminQueries(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<User> Users, int Total)> GetAllUsersAsync(
        string? search,
        string? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Users
            .Where(u => u.Role == UserRole.User)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(search))
        {
            var searchTerm = search.ToLower();
            query = query.Where(u =>
                u.Name.ToLower().Contains(searchTerm) ||
                u.Email!.ToLower().Contains(searchTerm) ||
                u.Mobile!.Contains(searchTerm));
        }

        if (!string.IsNullOrEmpty(status))
        {
            var isActive = status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase);
            query = query.Where(u => u.IsActive == isActive);
        }

        var total = await query.CountAsync(cancellationToken);
        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (users, total);
    }

    public async Task<int> GetUserBookingCountAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .CountAsync(b => b.CustomerId == userId, cancellationToken);
    }

    public async Task<User?> GetUserDetailsAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<(IEnumerable<VendorProfile> Vendors, int Total)> GetAllVendorsAsync(
        string? search,
        string? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.VendorProfiles
            .Include(vp => vp.User)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(search))
        {
            var searchTerm = search.ToLower();
            query = query.Where(vp =>
                vp.BusinessName.ToLower().Contains(searchTerm) ||
                vp.User!.Name.ToLower().Contains(searchTerm) ||
                vp.User.Email!.ToLower().Contains(searchTerm));
        }

        if (!string.IsNullOrEmpty(status))
        {
            var isActive = status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase);
            query = query.Where(vp => vp.User != null && vp.User.IsActive == isActive);
        }

        var total = await query.CountAsync(cancellationToken);
        var vendors = await query
            .OrderByDescending(vp => vp.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (vendors, total);
    }

    public async Task<VendorProfile?> GetVendorDetailsAsync(long vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.VendorProfiles
            .Include(vp => vp.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(vp => vp.Id == vendorId, cancellationToken);
    }

    public async Task<(IEnumerable<Listing> Listings, int Total)> GetPendingListingsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Listings
            .Include(l => l.Vendor)
                .ThenInclude(v => v!.User)
            .Where(l => l.ApprovalStatus == ApprovalStatus.Pending)
            .AsNoTracking();

        var total = await query.CountAsync(cancellationToken);
        var listings = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (listings, total);
    }

    public async Task<(IEnumerable<Listing> Listings, int Total)> GetPendingListingsForApprovalAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Listings
            .Include(l => l.Vendor)
                .ThenInclude(v => v!.User)
            .Where(l => l.ApprovalStatus == ApprovalStatus.Pending || l.ApprovalStatus == ApprovalStatus.NeedsChanges)
            .AsNoTracking();

        var total = await query.CountAsync(cancellationToken);
        var listings = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (listings, total);
    }

    public async Task<(IEnumerable<Booking> Bookings, int Total)> GetAllBookingsAsync(
        string? status,
        string? dateFrom,
        string? dateTo,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.Listing)
            .AsNoTracking();

        if (!string.IsNullOrEmpty(status))
        {
            var statusEnum = Enum.Parse<BookingStatus>(status, ignoreCase: true);
            query = query.Where(b => b.Status == statusEnum);
        }

        if (!string.IsNullOrEmpty(dateFrom) && DateOnly.TryParse(dateFrom, out var fromDate))
        {
            query = query.Where(b => b.StartDate >= fromDate);
        }

        if (!string.IsNullOrEmpty(dateTo) && DateOnly.TryParse(dateTo, out var toDate))
        {
            query = query.Where(b => b.StartDate <= toDate);
        }

        var total = await query.CountAsync(cancellationToken);
        var bookings = await query
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (bookings, total);
    }

    public async Task<(IEnumerable<Payout> Payouts, int Total)> GetAllPayoutsAsync(
        string? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Payouts
            .AsNoTracking();

        if (!string.IsNullOrEmpty(status))
        {
            var statusUpper = status.ToUpperInvariant();
            query = query.Where(p => p.Status.ToUpperInvariant() == statusUpper);
        }

        var total = await query.CountAsync(cancellationToken);
        var payouts = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (payouts, total);
    }

    public async Task<(IEnumerable<Review> Reviews, int Total)> GetAllReviewsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Reviews
            .Include(r => r.Customer)
            .Include(r => r.Listing)
            .AsNoTracking();

        var total = await query.CountAsync(cancellationToken);
        var reviews = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (reviews, total);
    }

    public async Task<IEnumerable<Listing>> GetPendingListingsForApprovalAsync(int limit, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .Include(l => l.Vendor)
                .ThenInclude(v => v!.User)
            .Where(l => l.ApprovalStatus == ApprovalStatus.Pending)
            .OrderByDescending(l => l.CreatedAt)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payout>> GetPendingPayoutsAsync(int limit, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Where(p => p.Status == "PENDING")
            .OrderByDescending(p => p.CreatedAt)
            .Take(limit)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalUsersCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .CountAsync(u => u.Role == UserRole.User, cancellationToken);
    }

    public async Task<int> GetActiveVendorsCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.VendorProfiles
            .AsNoTracking()
            .Include(vp => vp.User)
            .CountAsync(vp => vp.User != null && vp.User.IsActive, cancellationToken);
    }

    public async Task<int> GetTotalBookingsCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .AsNoTracking()
            .CountAsync(cancellationToken);
    }

    public async Task<decimal> GetPlatformRevenueAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.PaymentStatus == PaymentStatus.Paid)
            .AsNoTracking()
            .SumAsync(b => (decimal?)b.PlatformFee, cancellationToken) ?? 0;
    }

    public async Task<int> GetPendingListingsCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .AsNoTracking()
            .CountAsync(l => l.ApprovalStatus == ApprovalStatus.Pending, cancellationToken);
    }

    public async Task<int> GetPendingPayoutsCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .AsNoTracking()
            .CountAsync(p => p.Status == "PENDING", cancellationToken);
    }

    public async Task<int> GetVendorListingsCountAsync(long vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.Listings
            .AsNoTracking()
            .CountAsync(l => l.VendorId == vendorId, cancellationToken);
    }

    public async Task<decimal> GetVendorEarningsAsync(long vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.VendorId == vendorId && b.PaymentStatus == PaymentStatus.Paid)
            .AsNoTracking()
            .SumAsync((Booking b) => (decimal?)(b.TotalAmount - b.PlatformFee), cancellationToken) ?? 0;
    }

    public async Task<decimal> GetUserTotalSpentAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.CustomerId == userId && b.PaymentStatus == PaymentStatus.Paid)
            .AsNoTracking()
            .SumAsync((Booking b) => (decimal?)b.TotalAmount, cancellationToken) ?? 0;
    }

    public async Task<decimal> GetTotalRevenueAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.PaymentStatus == PaymentStatus.Paid &&
                       b.StartDate >= startDate &&
                       b.StartDate <= endDate)
            .AsNoTracking()
            .SumAsync((Booking b) => (decimal?)b.TotalAmount, cancellationToken) ?? 0;
    }

    public async Task<int> GetTotalBookingsCountAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.StartDate >= startDate && b.StartDate <= endDate)
            .AsNoTracking()
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetTotalUsersCountAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Role == UserRole.User &&
                       u.CreatedAt >= startDate.ToDateTime(TimeOnly.MinValue) &&
                       u.CreatedAt <= endDate.ToDateTime(TimeOnly.MaxValue))
            .AsNoTracking()
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetTotalVendorsCountAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _context.VendorProfiles
            .Where(vp => vp.CreatedAt >= startDate.ToDateTime(TimeOnly.MinValue) &&
                        vp.CreatedAt <= endDate.ToDateTime(TimeOnly.MaxValue))
            .AsNoTracking()
            .CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Role == UserRole.User &&
                       u.CreatedAt >= startDate.ToDateTime(TimeOnly.MinValue) &&
                       u.CreatedAt <= endDate.ToDateTime(TimeOnly.MaxValue))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.StartDate >= startDate && b.StartDate <= endDate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Booking>> GetCompletedBookingsByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _context.Bookings
            .Where(b => b.PaymentStatus == PaymentStatus.Paid &&
                       b.StartDate >= startDate &&
                       b.StartDate <= endDate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VendorProfile>> GetVendorsByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        return await _context.VendorProfiles
            .Where(vp => vp.CreatedAt >= startDate.ToDateTime(TimeOnly.MinValue) &&
                        vp.CreatedAt <= endDate.ToDateTime(TimeOnly.MaxValue))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}

