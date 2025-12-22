namespace SambhramaBook.Application.Models.Auth;

public class VerifyOtpRequest
{
    public required string MobileOrEmail { get; set; }
    public required string Otp { get; set; }
}

