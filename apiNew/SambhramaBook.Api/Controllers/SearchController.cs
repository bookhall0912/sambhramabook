using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Handlers.Search;
using SambhramaBook.Application.Models.Search;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    private readonly IGlobalSearchHandler _globalSearchHandler;
    private readonly ILogger<SearchController> _logger;

    public SearchController(
        IGlobalSearchHandler globalSearchHandler,
        ILogger<SearchController> logger)
    {
        _globalSearchHandler = globalSearchHandler;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GlobalSearchResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GlobalSearchResponse>> GlobalSearch(
        [FromQuery] string q,
        [FromQuery] string? type = "all",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GlobalSearchRequest
            {
                Query = q,
                Type = type ?? "all",
                Page = page,
                PageSize = pageSize
            };
            var response = await _globalSearchHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing global search");
            return StatusCode(500, new { success = false, message = "An error occurred" });
        }
    }
}

