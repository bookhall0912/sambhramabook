using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Models.Hall;
using SambhramaBook.Application.Queries;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HallsController : ControllerBase
{
    private readonly IHallQueries _hallQueries;
    private readonly ILogger<HallsController> _logger;

    public HallsController(
        IHallQueries hallQueries,
        ILogger<HallsController> logger)
    {
        _hallQueries = hallQueries;
        _logger = logger;
    }

    [HttpGet("popular")]
    public async Task<ActionResult<IEnumerable<HallListItemDto>>> GetPopularHalls([FromQuery] int limit = 3)
    {
        try
        {
            var halls = await _hallQueries.GetPopularHallsAsync(limit);
            return Ok(new { success = true, data = halls });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting popular halls");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet]
    public async Task<ActionResult<HallSearchResponseDto>> SearchHalls([FromQuery] HallSearchRequestDto request)
    {
        try
        {
            var response = await _hallQueries.SearchHallsAsync(request);
            return Ok(new { success = true, data = response.Halls, pagination = new { response.Total, response.Page, response.PageSize, response.TotalPages } });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching halls");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HallDetailDto>> GetHallById(long id)
    {
        try
        {
            var hall = await _hallQueries.GetHallByIdAsync(id);
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

    [HttpGet("{id}/availability")]
    public async Task<ActionResult<HallAvailabilityResponseDto>> GetAvailability(long id, [FromQuery] string month, [FromQuery] int year)
    {
        try
        {
            var availability = await _hallQueries.GetHallAvailabilityAsync(id, month, year);
            return Ok(new { success = true, data = availability });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting hall availability");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
}

