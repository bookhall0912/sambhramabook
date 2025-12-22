using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SambhramaBook.Application.Models.User;
using SambhramaBook.Application.Repositories;

namespace SambhramaBook.Application.Handlers.Auth;

public class GetCurrentUserHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCurrentUserHandler(
        IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<GetCurrentUserResponse?> HandleAsync(CancellationToken cancellationToken = default)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
        {
            return null;
        }

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return null;
        }

        return new GetCurrentUserResponse
        {
            Success = true,
            Data = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Mobile = user.Mobile ?? string.Empty,
                Role = user.Role,
                VendorProfileComplete = user.VendorProfile?.ProfileComplete ?? false
            }
        };
    }
}

