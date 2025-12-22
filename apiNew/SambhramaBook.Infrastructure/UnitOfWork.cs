using Microsoft.EntityFrameworkCore.Storage;
using SambhramaBook.Application.UnitOfWork;

namespace SambhramaBook.Infrastructure;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly SambhramaBookDbContext _db;

    public UnitOfWork(SambhramaBookDbContext db)
        => _db = db;

    public async Task SaveChanges(CancellationToken ct)
        => await _db.SaveChangesAsync(ct);

    public async Task ExecuteInTransaction(Func<Task> action, CancellationToken ct)
    {
        await using IDbContextTransaction transaction = await _db.Database.BeginTransactionAsync(ct);
        await action();
        try
        {
            await _db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}

