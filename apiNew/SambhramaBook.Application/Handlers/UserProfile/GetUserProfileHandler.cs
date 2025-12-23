using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.UserProfile;
using SambhramaBook.Application.Repositories;

namespace SambhramaBook.Application.Handlers.UserProfile;

public interface IGetUserProfileHandler
{
    Task<UserProfileDto?> Handle(long userId, CancellationToken ct);
}

public class GetUserProfileHandler : IGetUserProfileHandler
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserProfileDto?> Handle(long userId, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(userId, ct);
        if (user == null)
        {
            return null;
        }

        return new UserProfileDto
        {
            Id = user.Id.ToString(),
            Name = user.Name,
            Email = user.Email,
            Mobile = user.Mobile,
            Address = user.UserProfile?.AddressLine1,
            City = user.UserProfile?.City,
            State = user.UserProfile?.State,
            Pincode = user.UserProfile?.Pincode
        };
    }
}

