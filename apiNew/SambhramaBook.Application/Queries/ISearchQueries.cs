using SambhramaBook.Application.Models.Search;
using SambhramaBook.Domain.Entities;

namespace SambhramaBook.Application.Queries;

public interface ISearchQueries
{
    Task<IEnumerable<Listing>> SearchHallListingsAsync(string searchTerm, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Listing>> SearchServiceListingsAsync(string searchTerm, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entities.VendorProfile>> SearchVendorProfilesAsync(string searchTerm, int pageSize, CancellationToken cancellationToken = default);
}

