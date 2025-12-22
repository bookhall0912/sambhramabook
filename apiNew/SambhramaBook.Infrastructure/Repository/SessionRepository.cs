using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class SessionRepository : ISessionRepository
{
    private readonly SambhramaBookDbContext _context;

    public SessionRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<Session?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Session?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Token == token && s.ExpiresAt > DateTime.UtcNow, cancellationToken);
    }

    public async Task<Session?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _context.Sessions
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken && s.ExpiresAt > DateTime.UtcNow, cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return await _context.Sessions
            .Where(s => s.UserId == userId && s.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Session> CreateAsync(Session session, CancellationToken cancellationToken = default)
    {
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync(cancellationToken);
        return session;
    }

    public async Task<Session> UpdateAsync(Session session, CancellationToken cancellationToken = default)
    {
        session.LastActivityAt = DateTime.UtcNow;
        _context.Sessions.Update(session);
        await _context.SaveChangesAsync(cancellationToken);
        return session;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var session = await _context.Sessions.FindAsync([id], cancellationToken);
        if (session != null)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteExpiredSessionsAsync(CancellationToken cancellationToken = default)
    {
        var expiredSessions = await _context.Sessions
            .Where(s => s.ExpiresAt <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        _context.Sessions.RemoveRange(expiredSessions);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

