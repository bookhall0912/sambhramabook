using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Models.User;

public class UserDto
{
    public long Id { get; set; }
    public string Mobile { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Name { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool IsMobileVerified { get; set; }
    public bool VendorProfileComplete { get; set; }
}

