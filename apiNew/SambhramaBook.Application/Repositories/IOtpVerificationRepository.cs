using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface IOtpVerificationRepository
{
    Task<OtpVerification?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<OtpVerification?> GetLatestByMobileOrEmailAsync(string mobileOrEmail, string otpType, CancellationToken cancellationToken = default);
    Task<OtpVerification> CreateAsync(OtpVerification otpVerification, CancellationToken cancellationToken = default);
    Task<OtpVerification> UpdateAsync(OtpVerification otpVerification, CancellationToken cancellationToken = default);
    Task DeleteExpiredOtpsAsync(CancellationToken cancellationToken = default);
}

