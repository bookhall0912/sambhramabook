using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Api.Features.Service;
using SambhramaBook.Application.Handlers.Service;
using SambhramaBook.Domain;
using ServiceResponseDto = SambhramaBook.Application.Handlers.Service.ServiceResponseDto;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("services")]
public class ServiceController : ControllerBase
{
    private readonly IServiceCategoryGetHandler _serviceCategoryGetHandler;
    private readonly IServicesGetHandler _servicesGetHandler;

    public ServiceController(IServiceCategoryGetHandler serviceCategoryGetHandler, IServicesGetHandler servicesGetHandler)
    {
        _serviceCategoryGetHandler = serviceCategoryGetHandler;
        _servicesGetHandler = servicesGetHandler;
    }

    [HttpGet("categories"), ProducesResponseType<IEnumerable<ServiceCategoryResponseModel>>(StatusCodes.Status200OK)]
    public async Task<IEnumerable<ServiceCategoryResponseModel>> GetServiceCategories(CancellationToken ct)
    {
        ReadOnlyCollection<ServiceCategory> categories = await _serviceCategoryGetHandler.Handle(ct);

        return categories.Select(c => new ServiceCategoryResponseModel(c));
    }

    [HttpGet, ProducesResponseType<IEnumerable<ServiceGetResponseModel>>(StatusCodes.Status200OK)]
    public async Task<IEnumerable<ServiceGetResponseModel>> Get(ServicesGetRequestModel request, CancellationToken ct)
    {
        ReadOnlyCollection<ServiceResponseDto> services = await _servicesGetHandler.Handle(request.ToModel(), ct);

        return services.Select(s => new ServiceGetResponseModel(s));
    }
}
