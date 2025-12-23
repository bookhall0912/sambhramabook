using SambhramaBook.Application.Models.Auth;
using SambhramaBook.Application.Models.User;
using SambhramaBook.Application.Repositories;

namespace SambhramaBook.Application.Handlers.Auth;

public class GetCurrentUserHandler
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<GetCurrentUserResponse?> HandleAsync(long userId, CancellationToken cancellationToken = default)
    {
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

