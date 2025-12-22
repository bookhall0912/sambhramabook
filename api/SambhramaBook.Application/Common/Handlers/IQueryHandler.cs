namespace SambhramaBook.Application.Common.Handlers;

public interface IQueryHandler<TResult>
{
    Task<TResult> Handle(CancellationToken ct);
}

public interface IQueryHandler<TParam, TResult>
{
    Task<TResult> Handle(TParam param, CancellationToken ct);
}
