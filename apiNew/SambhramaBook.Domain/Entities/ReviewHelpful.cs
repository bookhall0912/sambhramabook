namespace SambhramaBook.Domain.Entities;

public class ReviewHelpful
{
    public long Id { get; set; }
    public long ReviewId { get; set; }
    public long UserId { get; set; }
    public required bool IsHelpful { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Review Review { get; set; } = null!;
    public User User { get; set; } = null!;
}

