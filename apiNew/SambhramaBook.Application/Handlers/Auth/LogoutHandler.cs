using SambhramaBook.Application.Models.Auth;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Application.Handlers.Auth;

public class LogoutHandler
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutHandler(
        ISessionRepository sessionRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LogoutResponse> HandleAsync(string token, CancellationToken cancellationToken = default)
    {
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

