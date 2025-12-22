namespace SambhramaBook.Application.Models.Hall;

public class HallSearchResponseDto
{
    public List<HallListItemDto> Halls { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

