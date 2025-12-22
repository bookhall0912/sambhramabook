namespace SambhramaBook.Domain.Entities;

public class OtpVerification
{
    public long Id { get; set; }
    public required string MobileOrEmail { get; set; }
    public required string OtpCode { get; set; }
    public required string OtpType { get; set; } // LOGIN, REGISTER, RESET_PASSWORD, VERIFY_EMAIL, VERIFY_MOBILE
    public bool IsUsed { get; set; } = false;
    public required DateTime ExpiresAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

