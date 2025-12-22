using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
    bool ValidateToken(string token);
    string? GetUserIdFromToken(string token);
}

