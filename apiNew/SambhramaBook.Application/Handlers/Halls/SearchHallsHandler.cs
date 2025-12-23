using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Hall;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Halls;

public interface ISearchHallsHandler : IQueryHandler<HallSearchRequestDto, HallSearchResponseDto>;

public class SearchHallsHandler : ISearchHallsHandler
{
    private readonly IHallQueries _hallQueries;

    public SearchHallsHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<HallSearchResponseDto> Handle(HallSearchRequestDto request, CancellationToken ct)
    {
        return await _hallQueries.SearchHallsAsync(request, ct);
    }
}

