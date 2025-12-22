namespace SambhramaBook.Domain.Entities;

public class Notification
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public required string NotificationType { get; set; } // 'BOOKING_CONFIRMED', 'PAYMENT_RECEIVED', etc.
    public required string Title { get; set; }
    public required string Message { get; set; }
    public string? Data { get; set; } // JSON data for additional context

    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }

    public string? ActionUrl { get; set; } // Deep link to relevant page
    public string Priority { get; set; } = "NORMAL"; // LOW, NORMAL, HIGH, URGENT

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public User User { get; set; } = null!;
}

