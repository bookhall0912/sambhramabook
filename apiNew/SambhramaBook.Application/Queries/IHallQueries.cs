using SambhramaBook.Application.Models.Hall;

namespace SambhramaBook.Application.Queries;

public interface IHallQueries
{
    Task<IEnumerable<HallListItemDto>> GetPopularHallsAsync(int limit, CancellationToken cancellationToken = default);
    Task<HallSearchResponseDto> SearchHallsAsync(HallSearchRequestDto request, CancellationToken cancellationToken = default);
    Task<HallDetailDto?> GetHallByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<HallDetailDto?> GetHallBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<HallAvailabilityResponseDto> GetHallAvailabilityAsync(long id, string month, int year, CancellationToken cancellationToken = default);
}

