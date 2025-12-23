using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Auth;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.Services;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Handlers.Auth;

public class SendOtpHandler
{
    private readonly IOtpVerificationRepository _otpRepository;
    private readonly IOtpService _otpService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<SendOtpHandler> _logger;

    public SendOtpHandler(
        IOtpVerificationRepository otpRepository,
        IOtpService otpService,
        IDateTimeProvider dateTimeProvider,
        ILogger<SendOtpHandler> logger)
    {
        _otpRepository = otpRepository;
        _otpService = otpService;
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

        // Send OTP via SMS/Email
        bool otpSent = false;
        if (IsPhoneNumber(request.MobileOrEmail))
        {
            otpSent = await _otpService.SendOtpSmsAsync(request.MobileOrEmail, otpCode, cancellationToken);
        }
        else if (IsEmail(request.MobileOrEmail))
        {
            otpSent = await _otpService.SendOtpEmailAsync(request.MobileOrEmail, otpCode, cancellationToken);
        }

        if (!otpSent)
        {
            _logger.LogWarning("OTP {OtpCode} generated but not sent to {MobileOrEmail}. Service may not be configured.", otpCode, request.MobileOrEmail);
        }

        return new SendOtpResponse
        {
            Success = true,
            Data = new SendOtpResponseData
            {
                OtpSent = otpSent,
                Message = otpSent ? "OTP sent successfully" : "OTP generated but service not configured",
                ExpiresIn = 300 // 5 minutes in seconds
            }
        };
    }

    private static bool IsPhoneNumber(string input)
    {
        // Check if input looks like a phone number (contains digits and possibly +)
        return !string.IsNullOrWhiteSpace(input) && 
               (input.StartsWith("+") || input.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' '));
    }

    private static bool IsEmail(string input)
    {
        return !string.IsNullOrWhiteSpace(input) && input.Contains("@") && input.Contains(".");
    }
}
