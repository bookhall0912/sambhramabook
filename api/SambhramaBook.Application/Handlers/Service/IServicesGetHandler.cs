using System.Collections.ObjectModel;
using SambhramaBook.Application.Common.Handlers;

namespace SambhramaBook.Application.Handlers.Service;

public interface IServicesGetHandler : IQueryHandler<ServiceGetModel, ReadOnlyCollection<ServiceResponseDto>>;
