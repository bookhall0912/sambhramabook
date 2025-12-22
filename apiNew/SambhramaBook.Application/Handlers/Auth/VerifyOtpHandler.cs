using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Auth;
using SambhramaBook.Application.Models.User;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.Services;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Auth;

public class VerifyOtpHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IOtpVerificationRepository _otpRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<VerifyOtpHandler> _logger;

    public VerifyOtpHandler(
        IUserRepository userRepository,
        IOtpVerificationRepository otpRepository,
        ISessionRepository sessionRepository,
        IJwtTokenService jwtTokenService,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ILogger<VerifyOtpHandler> logger)
    {
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _sessionRepository = sessionRepository;
        _jwtTokenService = jwtTokenService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<VerifyOtpResponse> HandleAsync(VerifyOtpRequest request, CancellationToken cancellationToken = default)
    {
        // Get latest OTP for the mobile/email
        var otpVerification = await _otpRepository.GetLatestByMobileOrEmailAsync(
            request.MobileOrEmail, 
            "LOGIN", 
            cancellationToken);

        if (otpVerification == null || otpVerification.OtpCode != request.Otp || otpVerification.IsUsed)
        {
            return new VerifyOtpResponse
            {
                Success = false,
                Error = new ErrorResponse
                {
                    Code = "INVALID_OTP",
                    Message = "Invalid or expired OTP"
                }
            };
        }

        // Check if OTP is expired
        if (otpVerification.ExpiresAt <= _dateTimeProvider.GetUtcNow())
        {
            return new VerifyOtpResponse
            {
                Success = false,
                Error = new ErrorResponse
                {
                    Code = "OTP_EXPIRED",
                    Message = "OTP has expired. Please request a new one."
                }
            };
        }

        // Mark OTP as used
        otpVerification.IsUsed = true;
        otpVerification.VerifiedAt = _dateTimeProvider.GetUtcNow();
        await _otpRepository.UpdateAsync(otpVerification, cancellationToken);

        // Clean phone number or email
        var isEmail = request.MobileOrEmail.Contains('@');
        var cleanPhone = isEmail ? null : new string(request.MobileOrEmail.Where(char.IsDigit).ToArray());

        // Check if user exists
        User? user;
        if (isEmail)
        {
            user = await _userRepository.GetByEmailAsync(request.MobileOrEmail, cancellationToken);
        }
        else
        {
            user = await _userRepository.GetByMobileAsync(cleanPhone!, cancellationToken);
        }

        // Create user if doesn't exist
        if (user == null)
        {
            user = new User
            {
                Mobile = cleanPhone,
                Email = isEmail ? request.MobileOrEmail : null,
                Name = "User",
                Role = UserRole.User,
                IsMobileVerified = !isEmail,
                IsEmailVerified = isEmail,
                IsActive = true,
                CreatedAt = _dateTimeProvider.GetUtcNow(),
                UpdatedAt = _dateTimeProvider.GetUtcNow()
            };

            user = await _userRepository.CreateAsync(user, cancellationToken);
        }
        else
        {
            // Update last login and verification status
            user.LastLoginAt = _dateTimeProvider.GetUtcNow();
            if (isEmail)
            {
                user.IsEmailVerified = true;
            }
            else
            {
                user.IsMobileVerified = true;
            }
            user = await _userRepository.UpdateAsync(user, cancellationToken);
        }

        // Generate JWT token
        var token = _jwtTokenService.GenerateToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // Create session
        var session = new Session
        {
            UserId = user.Id,
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = _dateTimeProvider.GetUtcNow().AddDays(7), // 7 days for refresh token
            CreatedAt = _dateTimeProvider.GetUtcNow(),
            LastActivityAt = _dateTimeProvider.GetUtcNow()
        };

        await _sessionRepository.CreateAsync(session, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new VerifyOtpResponse
        {
            Success = true,
            Data = new VerifyOtpResponseData
            {
                Token = token,
                RefreshToken = refreshToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Mobile = user.Mobile ?? string.Empty,
                    Role = user.Role,
                    VendorProfileComplete = user.VendorProfile?.ProfileComplete ?? false
                }
            }
        };
    }
}
