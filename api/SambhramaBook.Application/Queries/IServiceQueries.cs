using System.Collections.ObjectModel;
using SambhramaBook.Application.Handlers.Service;
using SambhramaBook.Domain;

namespace SambhramaBook.Application.Queries;

public interface IServiceQueries
{
    Task<ReadOnlyCollection<ServiceCategory>> GetCategories(CancellationToken ct);
    Task<ReadOnlyCollection<ServiceWithDetails>> GetServices(ServiceGetModel model, CancellationToken ct);
}
