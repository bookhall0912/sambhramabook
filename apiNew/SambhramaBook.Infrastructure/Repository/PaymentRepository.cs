using Microsoft.EntityFrameworkCore;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly SambhramaBookDbContext _context;

    public PaymentRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Include(p => p.Booking)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Payment?> GetByReferenceAsync(string reference, CancellationToken cancellationToken = default)
    {
        return await _context.Payments
            .Include(p => p.Booking)
            .FirstOrDefaultAsync(p => p.PaymentReference == reference, cancellationToken);
    }

    public async Task<Payment> CreateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);
        return payment;
    }

    public async Task<Payment> UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        payment.UpdatedAt = DateTime.UtcNow;
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync(cancellationToken);
        return payment;
    }
}

