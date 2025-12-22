using System.Collections.ObjectModel;
using SambhramaBook.Application.Queries;
using SambhramaBook.Domain;

namespace SambhramaBook.Application.Handlers.Service;

public sealed class ServiceCategoryGetHandler : IServiceCategoryGetHandler
{
    private readonly IServiceQueries _serviceQueries;

    public ServiceCategoryGetHandler(IServiceQueries serviceQueries)
    {
        _serviceQueries = serviceQueries;
    }

    public async Task<ReadOnlyCollection<ServiceCategory>> Handle(CancellationToken ct) =>
        await _serviceQueries.GetCategories(ct);
}
