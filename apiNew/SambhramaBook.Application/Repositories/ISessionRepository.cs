using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Session?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<Session?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<IEnumerable<Session>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);
    Task<Session> CreateAsync(Session session, CancellationToken cancellationToken = default);
    Task<Session> UpdateAsync(Session session, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    Task DeleteExpiredSessionsAsync(CancellationToken cancellationToken = default);
}

