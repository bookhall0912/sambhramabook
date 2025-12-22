using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Auth;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Handlers.Auth;

public class SendOtpHandler
{
    private readonly IOtpVerificationRepository _otpRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<SendOtpHandler> _logger;

    public SendOtpHandler(
        IOtpVerificationRepository otpRepository,
        IDateTimeProvider dateTimeProvider,
        ILogger<SendOtpHandler> logger)
    {
        _otpRepository = otpRepository;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<SendOtpResponse> HandleAsync(SendOtpRequest request, CancellationToken cancellationToken = default)
    {
        // Generate 6-digit OTP
        var random = new Random();
        var otpCode = random.Next(100000, 999999).ToString();

        // Store OTP in database
        var otpVerification = new OtpVerification
        {
            MobileOrEmail = request.MobileOrEmail,
            OtpCode = otpCode,
            OtpType = request.OtpType,
            IsUsed = false,
            ExpiresAt = _dateTimeProvider.GetUtcNow().AddMinutes(5), // 5 minutes expiry
            CreatedAt = _dateTimeProvider.GetUtcNow()
        };

        await _otpRepository.CreateAsync(otpVerification, cancellationToken);

        // TODO: Send OTP via SMS/Email service
        // For now, log it (remove in production)
        _logger.LogInformation("OTP {OtpCode} sent to {MobileOrEmail} (expires in 5 minutes)", otpCode, request.MobileOrEmail);

        return new SendOtpResponse
        {
            Success = true,
            Data = new SendOtpResponseData
            {
                OtpSent = true,
                Message = "OTP sent successfully",
                ExpiresIn = 300 // 5 minutes in seconds
            }
        };
    }
}
