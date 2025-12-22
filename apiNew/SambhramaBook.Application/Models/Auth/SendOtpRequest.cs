namespace SambhramaBook.Application.Models.Auth;

public class SendOtpRequest
{
    public required string MobileOrEmail { get; set; }
    public string OtpType { get; set; } = "LOGIN"; // LOGIN, REGISTER, RESET_PASSWORD
}

