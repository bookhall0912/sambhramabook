using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Booking?> GetByReferenceAsync(string reference, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByCustomerIdAsync(long customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Booking>> GetByListingIdAsync(long listingId, CancellationToken cancellationToken = default);
    Task<Booking> CreateAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<Booking> UpdateAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<string> GenerateBookingReferenceAsync(CancellationToken cancellationToken = default);
}

