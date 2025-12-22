using System.Collections.ObjectModel;
using SambhramaBook.Application.Common.Handlers;
using SambhramaBook.Domain;

namespace SambhramaBook.Application.Handlers.Service;

public interface IServiceCategoryGetHandler : IQueryHandler<ReadOnlyCollection<ServiceCategory>>;
