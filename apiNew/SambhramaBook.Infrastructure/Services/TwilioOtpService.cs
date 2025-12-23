using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SambhramaBook.Application.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SambhramaBook.Infrastructure.Services;

public class TwilioOtpService : IOtpService
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromPhoneNumber;
    private readonly ILogger<TwilioOtpService> _logger;
    private readonly bool _isConfigured;

    public TwilioOtpService(
        IConfiguration configuration,
        ILogger<TwilioOtpService> logger)
    {
        _logger = logger;
        _accountSid = configuration["Twilio:AccountSid"] ?? string.Empty;
        _authToken = configuration["Twilio:AuthToken"] ?? string.Empty;
        _fromPhoneNumber = configuration["Twilio:FromPhoneNumber"] ?? string.Empty;

        _isConfigured = !string.IsNullOrEmpty(_accountSid) && 
                       !string.IsNullOrEmpty(_authToken) && 
                       !string.IsNullOrEmpty(_fromPhoneNumber);

        if (_isConfigured)
        {
            TwilioClient.Init(_accountSid, _authToken);
        }
        else
        {
            _logger.LogWarning("Twilio is not configured. OTP SMS will not be sent. Please set Twilio:AccountSid, Twilio:AuthToken, and Twilio:FromPhoneNumber in appsettings.json");
        }
    }

    public async Task<bool> SendOtpSmsAsync(string phoneNumber, string otpCode, CancellationToken cancellationToken = default)
    {
        if (!_isConfigured)
        {
            _logger.LogWarning("Twilio not configured. OTP {OtpCode} would be sent to {PhoneNumber}", otpCode, phoneNumber);
            return false;
        }

        try
        {
            // Ensure phone number is in E.164 format
            var formattedPhone = FormatPhoneNumber(phoneNumber);

            var message = await MessageResource.CreateAsync(
                body: $"Your SambhramaBook verification code is: {otpCode}. This code expires in 5 minutes.",
                from: new Twilio.Types.PhoneNumber(_fromPhoneNumber),
                to: new Twilio.Types.PhoneNumber(formattedPhone)
            );

            _logger.LogInformation("OTP SMS sent successfully to {PhoneNumber}. Message SID: {MessageSid}", formattedPhone, message.Sid);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending OTP SMS to {PhoneNumber}", phoneNumber);
            return false;
        }
    }

    public async Task<bool> SendOtpEmailAsync(string email, string otpCode, CancellationToken cancellationToken = default)
    {
        // Email implementation can be added later using SendGrid, AWS SES, etc.
        _logger.LogWarning("Email OTP not implemented. OTP {OtpCode} would be sent to {Email}", otpCode, email);
        return await Task.FromResult(false);
    }

    private string FormatPhoneNumber(string phoneNumber)
    {
        // Remove all non-digit characters
        var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());

        // If it doesn't start with +, assume it's an Indian number and add +91
        if (!phoneNumber.StartsWith("+"))
        {
            if (digits.Length == 10)
            {
                return $"+91{digits}";
            }
            else if (digits.Length == 12 && digits.StartsWith("91"))
            {
                return $"+{digits}";
            }
        }

        return phoneNumber.StartsWith("+") ? phoneNumber : $"+{digits}";
    }
}

