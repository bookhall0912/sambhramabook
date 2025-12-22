using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Payment?> GetByReferenceAsync(string reference, CancellationToken cancellationToken = default);
    Task<Payment> CreateAsync(Payment payment, CancellationToken cancellationToken = default);
    Task<Payment> UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
}

