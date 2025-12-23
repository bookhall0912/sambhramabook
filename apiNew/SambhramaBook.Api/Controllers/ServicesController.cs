using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Handlers.LandingPage;
using SambhramaBook.Application.Handlers.Services;
using SambhramaBook.Application.Models.Services;
using SambhramaBook.Domain.Enums;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly IGetServiceCategoriesHandler _getServiceCategoriesHandler;
    private readonly IGetServicesByTypeHandler _getServicesByTypeHandler;
    private readonly IGetServiceDetailsHandler _getServiceDetailsHandler;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(
        IGetServiceCategoriesHandler getServiceCategoriesHandler,
        IGetServicesByTypeHandler getServicesByTypeHandler,
        IGetServiceDetailsHandler getServiceDetailsHandler,
        ILogger<ServicesController> logger)
    {
        _getServiceCategoriesHandler = getServiceCategoriesHandler;
        _getServicesByTypeHandler = getServicesByTypeHandler;
        _getServiceDetailsHandler = getServiceDetailsHandler;
        _logger = logger;
    }

    [HttpGet("categories")]
    [ProducesResponseType(typeof(ServiceCategoriesResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ServiceCategoriesResponse>> GetServiceCategories(CancellationToken ct = default)
    {
        try
        {
            var response = await _getServiceCategoriesHandler.Handle(ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service categories");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ServiceListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetServicesByLocation(
        [FromQuery] decimal? Longitude,
        [FromQuery] decimal? Lattitude, // Note: API uses "Lattitude" to match UI typo
        [FromQuery] int? ServiceType,
        [FromQuery] int? Limit,
        [FromQuery] decimal? Radius,
        [FromQuery] int? guestCount,
        [FromQuery] string? date,
        [FromQuery] int? days,
        CancellationToken ct = default)
    {
        try
        {
            // Convert ServiceType to ListingType
            // 1 = Hall, 2+ = Service (photography, catering, etc.)
            var listingType = ServiceType == 1 
                ? Domain.Enums.ListingType.Hall 
                : Domain.Enums.ListingType.Service;

            var limit = Limit ?? 12;
            var radiusKm = Radius ?? 50; // Default 50km radius

            // Get services/halls within radius
            var services = await _getServicesByTypeHandler.Handle(new GetServicesByTypeRequest
            {
                Type = ServiceType == 1 ? "hall" : "service",
                Location = null, // Location-based search uses coordinates
                Page = 1,
                PageSize = limit
            }, ct);

            // Filter by distance if coordinates provided
            List<ServiceListItemDto> filteredServices;
            if (Longitude.HasValue && Lattitude.HasValue)
            {
                filteredServices = services.Data
                    .Where(s => s.Latitude.HasValue && s.Longitude.HasValue)
                    .Select(s => new
                    {
                        Service = s,
                        Distance = CalculateDistance(
                            Lattitude.Value, Longitude.Value,
                            s.Latitude!.Value, s.Longitude!.Value)
                    })
                    .Where(x => x.Distance <= (double)radiusKm)
                    .OrderBy(x => x.Distance)
                    .Select(x => x.Service)
                    .ToList();
            }
            else
            {
                filteredServices = services.Data.ToList();
            }

            // Apply additional filters
            if (guestCount.HasValue)
            {
                // For halls, filter by capacity
                if (ServiceType == 1)
                {
                    // This would need capacity info in ServiceListItemDto
                    // For now, we'll skip this filter or add it later
                }
            }

            return Ok(new { success = true, data = filteredServices });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting services by location");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    // Haversine formula to calculate distance between two coordinates
    private static double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
    {
        const double R = 6371; // Earth's radius in kilometers
        var dLat = ToRadians((double)(lat2 - lat1));
        var dLon = ToRadians((double)(lon2 - lon1));
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians((double)lat1)) * Math.Cos(ToRadians((double)lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;

    [HttpGet("{type}")]
    [ProducesResponseType(typeof(GetServicesByTypeResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetServicesByTypeResponse>> GetServicesByType(
        string type,
        [FromQuery] string? location,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetServicesByTypeRequest
            {
                Type = type,
                Location = location,
                Page = page,
                PageSize = pageSize
            };
            var response = await _getServicesByTypeHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting services by type");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("{type}/{id}")]
    [ProducesResponseType(typeof(ApiResponse<ServiceDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetServiceDetails(string type, long id, CancellationToken ct = default)
    {
        try
        {
            var service = await _getServiceDetailsHandler.Handle(id, ct);
            if (service == null)
            {
                return NotFound(new { success = false, message = "Service not found" });
            }
            return Ok(new { success = true, data = service });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service details");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("{type}/{id}/packages")]
    [ProducesResponseType(typeof(ApiResponse<List<ServicePackageDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetServicePackages(string type, long id, CancellationToken ct = default)
    {
        try
        {
            var service = await _getServiceDetailsHandler.Handle(id, ct);
            if (service == null)
            {
                return NotFound(new { success = false, message = "Service not found" });
            }
            return Ok(new { success = true, data = service.Packages });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service packages");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("{type}/{id}/portfolio")]
    [ProducesResponseType(typeof(ApiResponse<ServicePortfolioResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetServicePortfolio(string type, long id, CancellationToken ct = default)
    {
        try
        {
            var service = await _getServiceDetailsHandler.Handle(id, ct);
            if (service == null)
            {
                return NotFound(new { success = false, message = "Service not found" });
            }
            return Ok(new { success = true, data = new ServicePortfolioResponse { Images = service.PortfolioImages } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service portfolio");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("{type}/{id}/reviews")]
    [ProducesResponseType(typeof(ApiResponse<List<ServiceReviewDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetServiceReviews(
        string type,
        long id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var service = await _getServiceDetailsHandler.Handle(id, ct);
            if (service == null)
            {
                return NotFound(new { success = false, message = "Service not found" });
            }
            var pagedReviews = service.Reviews
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return Ok(new { success = true, data = pagedReviews });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting service reviews");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
}


