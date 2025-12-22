using SambhramaBook.Application.Models.User;

namespace SambhramaBook.Application.Models.Auth;

public class VerifyOtpResponse
{
    public bool Success { get; set; }
    public VerifyOtpResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class VerifyOtpResponseData
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}

public class ErrorResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
