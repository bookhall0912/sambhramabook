using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<User?> GetByMobileAsync(string mobile, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> ExistsByMobileAsync(string mobile, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}

