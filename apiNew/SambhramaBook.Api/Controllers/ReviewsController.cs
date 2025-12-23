using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Handlers.Reviews;
using SambhramaBook.Application.Models.Reviews;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly CreateReviewHandler _createReviewHandler;
    private readonly IGetReviewsForListingHandler _getReviewsForListingHandler;
    private readonly MarkReviewHelpfulHandler _markReviewHelpfulHandler;
    private readonly AddVendorResponseHandler _addVendorResponseHandler;
    private readonly ILogger<ReviewsController> _logger;

    public ReviewsController(
        CreateReviewHandler createReviewHandler,
        IGetReviewsForListingHandler getReviewsForListingHandler,
        MarkReviewHelpfulHandler markReviewHelpfulHandler,
        AddVendorResponseHandler addVendorResponseHandler,
        ILogger<ReviewsController> logger)
    {
        _createReviewHandler = createReviewHandler;
        _getReviewsForListingHandler = getReviewsForListingHandler;
        _markReviewHelpfulHandler = markReviewHelpfulHandler;
        _addVendorResponseHandler = addVendorResponseHandler;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateReviewResponse>> CreateReview(
        [FromBody] CreateReviewRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _createReviewHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating review");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("listing/{listingId}")]
    [ProducesResponseType(typeof(GetReviewsForListingResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetReviewsForListingResponse>> GetReviewsForListing(
        long listingId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetReviewsForListingRequest
            {
                ListingId = listingId,
                Page = page,
                PageSize = pageSize
            };
            var response = await _getReviewsForListingHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reviews for listing");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("{id}/helpful")]
    [Authorize]
    [ProducesResponseType(typeof(MarkReviewHelpfulResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MarkReviewHelpfulResponse>> MarkReviewHelpful(
        long id,
        [FromBody] MarkReviewHelpfulRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _markReviewHelpfulHandler.HandleAsync(userId, id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking review as helpful");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("{id}/vendor-response")]
    [Authorize]
    [ProducesResponseType(typeof(AddVendorResponseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AddVendorResponseResponse>> AddVendorResponse(
        long id,
        [FromBody] AddVendorResponseRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _addVendorResponseHandler.HandleAsync(userId, id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding vendor response");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}

