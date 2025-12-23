using SambhramaBook.Application.Common;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Search;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.Search;

public interface IGlobalSearchHandler : IQueryHandler<GlobalSearchRequest, GlobalSearchResponse>;

public class GlobalSearchHandler : IGlobalSearchHandler
{
    private readonly ISearchQueries _searchQueries;

    public GlobalSearchHandler(ISearchQueries searchQueries)
    {
        _searchQueries = searchQueries;
    }

    public async Task<GlobalSearchResponse> Handle(GlobalSearchRequest request, CancellationToken ct)
    {
        var searchTerm = request.Query.ToLower();

        var halls = new List<SearchResultDto>();
        var services = new List<SearchResultDto>();
        var vendors = new List<SearchResultDto>();

        if (request.Type == "all" || request.Type == "hall")
        {
            var hallListings = await _searchQueries.SearchHallListingsAsync(searchTerm, request.PageSize, ct);
            halls = hallListings.Select(l => new SearchResultDto
            {
                Id = l.Id.ToString(),
                Name = l.Title,
                Location = l.City,
                Type = "hall"
            }).ToList();
        }

        if (request.Type == "all" || request.Type == "service")
        {
            var serviceListings = await _searchQueries.SearchServiceListingsAsync(searchTerm, request.PageSize, ct);
            services = serviceListings.Select(l => new SearchResultDto
            {
                Id = l.Id.ToString(),
                Name = l.Title,
                Location = l.City,
                Type = "service"
            }).ToList();
        }

        if (request.Type == "all" || request.Type == "vendor")
        {
            var vendorProfiles = await _searchQueries.SearchVendorProfilesAsync(searchTerm, request.PageSize, ct);
            vendors = vendorProfiles.Select(vp => new SearchResultDto
            {
                Id = vp.Id.ToString(),
                Name = vp.BusinessName,
                Location = vp.City,
                Type = "vendor"
            }).ToList();
        }

        var total = halls.Count + services.Count + vendors.Count;

        return new GlobalSearchResponse
        {
            Success = true,
            Data = new GlobalSearchData
            {
                Halls = halls,
                Services = services,
                Vendors = vendors
            },
            Pagination = new PaginationInfo
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling(total / (double)request.PageSize)
            }
        };
    }
}

