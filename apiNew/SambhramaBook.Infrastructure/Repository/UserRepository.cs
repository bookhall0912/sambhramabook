using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Repositories;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly SambhramaBookDbContext _context;

    public UserRepository(SambhramaBookDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.UserProfile)
            .Include(u => u.VendorProfile)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByMobileAsync(string mobile, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.UserProfile)
            .Include(u => u.VendorProfile)
            .FirstOrDefaultAsync(u => u.Mobile == mobile, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.UserProfile)
            .Include(u => u.VendorProfile)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<bool> ExistsByMobileAsync(string mobile, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AnyAsync(u => u.Mobile == mobile, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }
}

