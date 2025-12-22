using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class PayoutRepository : IPayoutRepository
{
    private readonly SambhramaBookDbContext _context;

    public PayoutRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<Payout?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Include(p => p.Vendor)
            .Include(p => p.Booking)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Payout>> GetByVendorIdAsync(long vendorId, CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Include(p => p.Booking)
            .Where(p => p.VendorId == vendorId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payout>> GetPendingPayoutsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Payouts
            .Include(p => p.Vendor)
            .Where(p => p.Status == "PENDING")
            .OrderBy(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Payout> CreateAsync(Payout payout, CancellationToken cancellationToken = default)
    {
        _context.Payouts.Add(payout);
        await _context.SaveChangesAsync(cancellationToken);
        return payout;
    }

    public async Task<Payout> UpdateAsync(Payout payout, CancellationToken cancellationToken = default)
    {
        payout.UpdatedAt = DateTime.UtcNow;
        _context.Payouts.Update(payout);
        await _context.SaveChangesAsync(cancellationToken);
        return payout;
    }
}

