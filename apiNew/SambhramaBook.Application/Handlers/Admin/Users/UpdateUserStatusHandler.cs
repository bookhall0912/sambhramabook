using SambhramaBook.Application.Common;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Application.Handlers.Admin.Users;

public class UpdateUserStatusHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateUserStatusHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<UpdateUserStatusResponse> HandleAsync(long userId, UpdateUserStatusRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return new UpdateUserStatusResponse
            {
                Success = false,
                Error = new ErrorResponse { Code = "NOT_FOUND", Message = "User not found" }
            };
        }

        user.IsActive = request.Status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase);
        user.UpdatedAt = _dateTimeProvider.GetUtcNow();

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new UpdateUserStatusResponse
        {
            Success = true,
            Data = new UpdateUserStatusResponseData
            {
                Id = user.Id.ToString(),
                Status = request.Status,
                Message = "User status updated"
            }
        };
    }
}

