using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Queries;

public interface IVendorQueries
{
    Task<Listing?> GetVendorListingDetailsAsync(long listingId, long vendorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ListingAvailability>> GetVendorAvailabilityAsync(
        long listingId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetVendorBookingsForAvailabilityAsync(
        long listingId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
    Task<(IEnumerable<Payout> Payouts, int Total)> GetPayoutHistoryAsync(
        long vendorId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<(IEnumerable<Payment> Transactions, int Total)> GetEarningsTransactionsAsync(
        long vendorId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<VendorProfile?> GetVendorSettingsAsync(long vendorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetVendorBookingsForEarningsAsync(
        long vendorId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
    Task<decimal> GetPendingPayoutsAmountAsync(long vendorId, CancellationToken cancellationToken = default);
}

