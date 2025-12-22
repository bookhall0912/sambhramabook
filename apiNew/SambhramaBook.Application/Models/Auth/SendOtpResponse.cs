namespace SambhramaBook.Application.Models.Auth;

public class SendOtpResponse
{
    public bool Success { get; set; }
    public SendOtpResponseData? Data { get; set; }
    public string? Message { get; set; }
}

public class SendOtpResponseData
{
    public bool OtpSent { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ExpiresIn { get; set; } // seconds
}
