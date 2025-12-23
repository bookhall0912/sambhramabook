using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.UserProfile;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Handlers.UserProfile;

public class UpdateUserProfileHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateUserProfileHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UpdateUserProfileResponse> HandleAsync(long userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return new UpdateUserProfileResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "User not found" }
            };
        }

        user.Name = request.FullName;
        user.Email = request.Email;
        user.Mobile = request.Mobile;
        user.UpdatedAt = _dateTimeProvider.GetUtcNow();

        if (user.UserProfile == null)
        {
            user.UserProfile = new Domain.Entities.UserProfile
            {
                UserId = userId,
                CreatedAt = _dateTimeProvider.GetUtcNow(),
                UpdatedAt = _dateTimeProvider.GetUtcNow()
            };
        }

        user.UserProfile.AddressLine1 = request.Address;
        user.UserProfile.City = request.City;
        user.UserProfile.State = request.State;
        user.UserProfile.Pincode = request.Pincode;
        user.UserProfile.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new UpdateUserProfileResponse
        {
            Success = true,
            Data = new UpdateUserProfileResponseData
            {
                Id = user.Id.ToString(),
                Message = "Profile updated successfully"
            }
        };
    }
}

