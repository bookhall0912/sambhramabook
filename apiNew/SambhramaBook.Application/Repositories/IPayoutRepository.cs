using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface IPayoutRepository
{
    Task<Payout?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payout>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payout>> GetPendingPayoutsAsync(CancellationToken cancellationToken = default);
    Task<Payout> CreateAsync(Payout payout, CancellationToken cancellationToken = default);
    Task<Payout> UpdateAsync(Payout payout, CancellationToken cancellationToken = default);
}

