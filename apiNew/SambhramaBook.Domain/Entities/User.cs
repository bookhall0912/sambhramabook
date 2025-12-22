using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Domain.Entities;

public class User
{
    public long Id { get; set; }
    public required string Mobile { get; set; }
    public string? Email { get; set; }
    public required string Name { get; set; }
    public UserRole Role { get; set; }
    public string? PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; } = false;
    public bool IsMobileVerified { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public UserProfile? UserProfile { get; set; }
    public VendorProfile? VendorProfile { get; set; }
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Review> Reviews { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
    public ICollection<SavedListing> SavedListings { get; set; } = [];
    public ICollection<Session> Sessions { get; set; } = [];
}

