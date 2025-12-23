using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Hall;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Halls;

public interface IGetHallByIdHandler : IQueryHandler<long, HallDetailDto?>;

public class GetHallByIdHandler : IGetHallByIdHandler
{
    private readonly IHallQueries _hallQueries;

    public GetHallByIdHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<HallDetailDto?> Handle(long id, CancellationToken ct)
    {
        return await _hallQueries.GetHallByIdAsync(id, ct);
    }
}

