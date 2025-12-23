using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Hall;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Halls;

public interface IGetHallAvailabilityHandler : IQueryHandler<GetHallAvailabilityRequest, HallAvailabilityResponseDto>;

public class GetHallAvailabilityHandler : IGetHallAvailabilityHandler
{
    private readonly IHallQueries _hallQueries;

    public GetHallAvailabilityHandler(IHallQueries hallQueries)
    {
        _hallQueries = hallQueries;
    }

    public async Task<HallAvailabilityResponseDto> Handle(GetHallAvailabilityRequest request, CancellationToken ct)
    {
        return await _hallQueries.GetHallAvailabilityAsync(request.HallId, request.Month, request.Year, ct);
    }
}

