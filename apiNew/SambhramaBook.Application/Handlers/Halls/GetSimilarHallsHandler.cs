using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Hall;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Halls;

public class GetSimilarHallsRequest
{
    public long HallId { get; set; }
    public int Limit { get; set; } = 4;
}

public interface IGetSimilarHallsHandler : IQueryHandler<GetSimilarHallsRequest, IEnumerable<HallListItemDto>>;

public class GetSimilarHallsHandler : IGetSimilarHallsHandler
{
    private readonly IHallQueries _hallQueries;

    public GetSimilarHallsHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<IEnumerable<HallListItemDto>> Handle(GetSimilarHallsRequest request, CancellationToken ct)
    {
        return await _hallQueries.GetSimilarHallsAsync(request.HallId, request.Limit, ct);
    }
}

