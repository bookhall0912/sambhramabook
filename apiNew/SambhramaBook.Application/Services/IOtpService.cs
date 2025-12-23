namespace SambhramaBook.Application.Services;

public interface IOtpService
{
    /// <summary>
    /// Sends OTP via SMS using Twilio
    /// </summary>
    /// <param name="phoneNumber">Phone number in E.164 format (e.g., +1234567890)</param>
    /// <param name="otpCode">The OTP code to send</param>
    /// <returns>True if sent successfully, false otherwise</returns>
    Task<bool> SendOtpSmsAsync(string phoneNumber, string otpCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends OTP via Email (optional implementation)
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="otpCode">The OTP code to send</param>
    /// <returns>True if sent successfully, false otherwise</returns>
    Task<bool> SendOtpEmailAsync(string email, string otpCode, CancellationToken cancellationToken = default);
}

