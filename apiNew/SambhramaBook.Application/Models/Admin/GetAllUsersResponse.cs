using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Models.Admin;

public class GetAllUsersResponse
{
    public bool Success { get; set; }
    public List<AdminUserDto> Data { get; set; } = [];
    public PaginationInfo? Pagination { get; set; }
}

public class AdminUserDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? Avatar { get; set; }
    public string Status { get; set; } = string.Empty;
    public string JoinDate { get; set; } = string.Empty;
    public int Bookings { get; set; }
}

