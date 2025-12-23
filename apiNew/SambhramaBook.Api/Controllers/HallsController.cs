using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Handlers.Halls;
using SambhramaBook.Application.Handlers.LandingPage;
using SambhramaBook.Application.Handlers.Reviews;
using SambhramaBook.Application.Models.Hall;
using SambhramaBook.Application.Models.Reviews;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HallsController : ControllerBase
{
    private readonly IGetPopularHallsHandler _getPopularHallsHandler;
    private readonly ISearchHallsHandler _searchHallsHandler;
    private readonly IGetHallByIdHandler _getHallByIdHandler;
    private readonly IGetHallBySlugHandler _getHallBySlugHandler;
    private readonly IGetHallAvailabilityHandler _getHallAvailabilityHandler;
    private readonly IGetReviewsForListingHandler _getReviewsForListingHandler;
    private readonly IGetSimilarHallsHandler _getSimilarHallsHandler;
    private readonly ILogger<HallsController> _logger;

    public HallsController(
        IGetPopularHallsHandler getPopularHallsHandler,
        ISearchHallsHandler searchHallsHandler,
        IGetHallByIdHandler getHallByIdHandler,
        IGetHallBySlugHandler getHallBySlugHandler,
        IGetHallAvailabilityHandler getHallAvailabilityHandler,
        IGetReviewsForListingHandler getReviewsForListingHandler,
        IGetSimilarHallsHandler getSimilarHallsHandler,
        ILogger<HallsController> logger)
    {
        _getPopularHallsHandler = getPopularHallsHandler;
        _searchHallsHandler = searchHallsHandler;
        _getHallByIdHandler = getHallByIdHandler;
        _getHallBySlugHandler = getHallBySlugHandler;
        _getHallAvailabilityHandler = getHallAvailabilityHandler;
        _getReviewsForListingHandler = getReviewsForListingHandler;
        _getSimilarHallsHandler = getSimilarHallsHandler;
        _logger = logger;
    }

    [HttpGet("popular")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HallListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetPopularHalls([FromQuery] int limit = 3, CancellationToken ct = default)
    {
        try
        {
            var halls = await _getPopularHallsHandler.Handle(limit, ct);
            return Ok(new { success = true, data = halls });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting popular halls");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<HallSearchResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> SearchHalls([FromQuery] HallSearchRequestDto request, CancellationToken ct = default)
    {
        try
        {
            var response = await _searchHallsHandler.Handle(request, ct);
            return Ok(new { success = true, data = response.Halls, pagination = new { response.Total, response.Page, response.PageSize, response.TotalPages } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching halls");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<HallDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetHallById(long id, CancellationToken ct = default)
    {
        try
        {
            var hall = await _getHallByIdHandler.Handle(id, ct);
            if (hall == null)
            {
                return NotFound(new { success = false, message = "Hall not found" });
            }
            return Ok(new { success = true, data = hall });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting hall by id");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("slug/{slug}")]
    [ProducesResponseType(typeof(ApiResponse<HallDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetHallBySlug(string slug, CancellationToken ct = default)
    {
        try
        {
            var hall = await _getHallBySlugHandler.Handle(slug, ct);
            if (hall == null)
            {
                return NotFound(new { success = false, message = "Hall not found" });
            }
            return Ok(new { success = true, data = hall });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting hall by slug");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("{id}/availability")]
    [ProducesResponseType(typeof(ApiResponse<HallAvailabilityResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAvailability(long id, [FromQuery] string month, [FromQuery] int year, CancellationToken ct = default)
    {
        try
        {
            var request = new GetHallAvailabilityRequest { HallId = id, Month = month, Year = year };
            var availability = await _getHallAvailabilityHandler.Handle(request, ct);
            return Ok(new { success = true, data = availability });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting hall availability");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("{id}/reviews")]
    [ProducesResponseType(typeof(GetReviewsForListingResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetReviewsForListingResponse>> GetHallReviews(
        long id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetReviewsForListingRequest
            {
                ListingId = id,
                Page = page,
                PageSize = pageSize
            };
            var response = await _getReviewsForListingHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting hall reviews");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("{id}/similar")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<HallListItemDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetSimilarHalls(
        long id,
        [FromQuery] int limit = 4,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetSimilarHallsRequest { HallId = id, Limit = limit };
            var halls = await _getSimilarHallsHandler.Handle(request, ct);
            return Ok(new { success = true, data = halls });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting similar halls");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
}

