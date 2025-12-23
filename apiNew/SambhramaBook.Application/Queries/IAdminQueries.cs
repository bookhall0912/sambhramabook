using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Queries;

public interface IAdminQueries
{
    Task<(IEnumerable<User> Users, int Total)> GetAllUsersAsync(
        string? search,
        string? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<int> GetUserBookingCountAsync(long userId, CancellationToken cancellationToken = default);
    Task<User?> GetUserDetailsAsync(long userId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<VendorProfile> Vendors, int Total)> GetAllVendorsAsync(
        string? search,
        string? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<VendorProfile?> GetVendorDetailsAsync(long vendorId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Listing> Listings, int Total)> GetPendingListingsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<(IEnumerable<Listing> Listings, int Total)> GetPendingListingsForApprovalAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<(IEnumerable<Booking> Bookings, int Total)> GetAllBookingsAsync(
        string? status,
        string? dateFrom,
        string? dateTo,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<(IEnumerable<Payout> Payouts, int Total)> GetAllPayoutsAsync(
        string? status,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<(IEnumerable<Review> Reviews, int Total)> GetAllReviewsAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<Listing>> GetPendingListingsForApprovalAsync(int limit, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payout>> GetPendingPayoutsAsync(int limit, CancellationToken cancellationToken = default);
    
    // Dashboard statistics
    Task<int> GetTotalUsersCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetActiveVendorsCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetTotalBookingsCountAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetPlatformRevenueAsync(CancellationToken cancellationToken = default);
    Task<int> GetPendingListingsCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetPendingPayoutsCountAsync(CancellationToken cancellationToken = default);
    Task<int> GetVendorListingsCountAsync(long vendorId, CancellationToken cancellationToken = default);
    Task<decimal> GetVendorEarningsAsync(long vendorId, CancellationToken cancellationToken = default);
    Task<decimal> GetUserTotalSpentAsync(long userId, CancellationToken cancellationToken = default);
    
    // Analytics methods
    Task<decimal> GetTotalRevenueAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<int> GetTotalBookingsCountAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<int> GetTotalUsersCountAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<int> GetTotalVendorsCountAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetCompletedBookingsByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<VendorProfile>> GetVendorsByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
}

