using SambhramaBook.Application.Models.User;

namespace SambhramaBook.Application.Models.Auth;

public class GetCurrentUserResponse
{
    public bool Success { get; set; }
    public UserDto Data { get; set; } = null!;
}

