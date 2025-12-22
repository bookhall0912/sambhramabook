using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Application.Handlers.Auth;

public class LogoutHandler
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutHandler(
        ISessionRepository sessionRepository,
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task<LogoutResponse> HandleAsync(CancellationToken cancellationToken = default)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);

        if (!string.IsNullOrEmpty(token))
        {
            var session = await _sessionRepository.GetByTokenAsync(token, cancellationToken);
            if (session != null)
            {
                await _sessionRepository.DeleteAsync(session.Id, cancellationToken);
            }
        }

        await _unitOfWork.SaveChanges(cancellationToken);

        return new LogoutResponse
        {
            Success = true,
            Message = "Logged out successfully"
        };
    }
}

