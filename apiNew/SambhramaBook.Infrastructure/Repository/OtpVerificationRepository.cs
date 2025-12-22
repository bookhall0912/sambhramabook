using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class OtpVerificationRepository : IOtpVerificationRepository
{
    private readonly SambhramaBookDbContext _context;

    public OtpVerificationRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<OtpVerification?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.OtpVerifications
            .FirstOrDefaultAsync(ov => ov.Id == id, cancellationToken);
    }

    public async Task<OtpVerification?> GetLatestByMobileOrEmailAsync(string mobileOrEmail, string otpType, CancellationToken cancellationToken = default)
    {
        return await _context.OtpVerifications
            .Where(ov => ov.MobileOrEmail == mobileOrEmail && 
                       ov.OtpType == otpType && 
                       !ov.IsUsed &&
                       ov.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(ov => ov.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<OtpVerification> CreateAsync(OtpVerification otpVerification, CancellationToken cancellationToken = default)
    {
        _context.OtpVerifications.Add(otpVerification);
        await _context.SaveChangesAsync(cancellationToken);
        return otpVerification;
    }

    public async Task<OtpVerification> UpdateAsync(OtpVerification otpVerification, CancellationToken cancellationToken = default)
    {
        _context.OtpVerifications.Update(otpVerification);
        await _context.SaveChangesAsync(cancellationToken);
        return otpVerification;
    }

    public async Task DeleteExpiredOtpsAsync(CancellationToken cancellationToken = default)
    {
        var expiredOtps = await _context.OtpVerifications
            .Where(ov => ov.ExpiresAt <= DateTime.UtcNow || ov.IsUsed)
            .ToListAsync(cancellationToken);

        _context.OtpVerifications.RemoveRange(expiredOtps);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

