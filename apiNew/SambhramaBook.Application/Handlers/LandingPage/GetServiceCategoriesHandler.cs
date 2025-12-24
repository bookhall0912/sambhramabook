using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Application.Models.Services;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Application.Handlers.LandingPage;

public interface IGetServiceCategoriesHandler : IQueryHandler<ServiceCategoriesResponse>;

public class GetServiceCategoriesHandler : IGetServiceCategoriesHandler
{
    private readonly IServiceQueries _serviceQueries;

    public GetServiceCategoriesHandler(IServiceQueries serviceQueries)
    {
        _serviceQueries = serviceQueries;
    }

    public async Task<ServiceCategoriesResponse> Handle(CancellationToken ct)
    {
        var categories = await _serviceQueries.GetServiceCategoriesAsync(ct);

        return new ServiceCategoriesResponse
        {
            Success = true,
            Data = new ServiceCategoriesResponseData
            {
                Categories = categories
            }
        };
    }
}

