namespace SambhramaBook.Domain.Entities;

public class Session
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public required string Token { get; set; }
    public string? RefreshToken { get; set; }
    public string? DeviceInfo { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public User User { get; set; } = null!;
}

