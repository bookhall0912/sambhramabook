using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.UserProfile;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.Services;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Application.Handlers.UserProfile;

public class ChangePasswordHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ChangePasswordHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ChangePasswordResponse> HandleAsync(long userId, ChangePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "User not found" }
            };
        }

        // Verify current password if it exists
        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Error = new ErrorResponse { Code = "INVALID_REQUEST", Message = "Current password is required" }
                };
            }

            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                return new ChangePasswordResponse
                {
                    Success = false,
                    Error = new ErrorResponse { Code = "INVALID_PASSWORD", Message = "Current password is incorrect" }
                };
            }
        }

        // Validate new password
        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6)
        {
            return new ChangePasswordResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "INVALID_REQUEST", Message = "New password must be at least 6 characters long" }
            };
        }

        // Hash and store new password
        user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        user.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new ChangePasswordResponse
        {
            Success = true,
            Message = "Password changed successfully"
        };
    }
}

