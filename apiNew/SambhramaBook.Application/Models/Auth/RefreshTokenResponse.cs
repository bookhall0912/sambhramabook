namespace SambhramaBook.Application.Models.Auth;

public class RefreshTokenResponse
{
    public bool Success { get; set; }
    public RefreshTokenResponseData? Data { get; set; }
    public ErrorResponse? Error { get; set; }
}

public class RefreshTokenResponseData
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

