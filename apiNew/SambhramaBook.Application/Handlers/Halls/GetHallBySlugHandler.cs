using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Hall;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Halls;

public interface IGetHallBySlugHandler : IQueryHandler<string, HallDetailDto?>;

public class GetHallBySlugHandler : IGetHallBySlugHandler
{
    private readonly IHallQueries _hallQueries;

    public GetHallBySlugHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<HallDetailDto?> Handle(string slug, CancellationToken ct)
    {
        return await _hallQueries.GetHallBySlugAsync(slug, ct);
    }
}

