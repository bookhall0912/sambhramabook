using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Hall;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.LandingPage;

public interface IGetPopularHallsHandler : IQueryHandler<int, IEnumerable<HallListItemDto>>;

public class GetPopularHallsHandler : IGetPopularHallsHandler
{
    private readonly IHallQueries _hallQueries;

    public GetPopularHallsHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<IEnumerable<HallListItemDto>> Handle(int limit, CancellationToken ct)
    {
        return await _hallQueries.GetPopularHallsAsync(limit, ct);
    }
}

