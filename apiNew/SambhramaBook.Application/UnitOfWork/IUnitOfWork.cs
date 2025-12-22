namespace SambhramaBook.Application.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveChanges(CancellationToken ct);
    Task ExecuteInTransaction(Func<Task> action, CancellationToken ct);
}

