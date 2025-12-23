using SambhramaBook.Application.Models.Auth;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.Services;
using SambhramaBook.Application.UnitOfWork;
using SambhramaBook.Application.Common;

namespace SambhramaBook.Application.Handlers.Auth;

public class RefreshTokenHandler
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RefreshTokenHandler(
        ISessionRepository sessionRepository,
        IJwtTokenService jwtTokenService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _sessionRepository = sessionRepository;
        _jwtTokenService = jwtTokenService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<RefreshTokenResponse> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (session == null)
        {
            return new RefreshTokenResponse
            {
                Success = false,
                Error = new Models.Auth.ErrorResponse
                {
                    Code = "INVALID_REFRESH_TOKEN",
                    Message = "Invalid or expired refresh token"
                }
            };
        }

        var user = await _userRepository.GetByIdAsync(session.UserId, cancellationToken);
        if (user == null || !user.IsActive)
        {
            return new RefreshTokenResponse
            {
                Success = false,
                Error = new Models.Auth.ErrorResponse
                {
                    Code = "USER_NOT_FOUND",
                    Message = "User not found or inactive"
                }
            };
        }

        // Generate new tokens
        var newToken = _jwtTokenService.GenerateToken(user);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        // Update session
        session.Token = newToken;
        session.RefreshToken = newRefreshToken;
        session.ExpiresAt = _dateTimeProvider.GetUtcNow().AddDays(7);
        session.LastActivityAt = _dateTimeProvider.GetUtcNow();

        await _sessionRepository.UpdateAsync(session, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);

        return new RefreshTokenResponse
        {
            Success = true,
            Data = new RefreshTokenResponseData
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            }
        };
    }
}

