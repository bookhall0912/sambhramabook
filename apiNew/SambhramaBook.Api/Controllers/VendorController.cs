using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Handlers.Vendor;
using SambhramaBook.Application.Handlers.VendorAvailability;
using SambhramaBook.Application.Handlers.VendorBookings;
using SambhramaBook.Application.Handlers.VendorDashboard;
using SambhramaBook.Application.Handlers.VendorEarnings;
using SambhramaBook.Application.Handlers.VendorListings;
using SambhramaBook.Application.Handlers.VendorSettings;
using SambhramaBook.Application.Models.Vendor;
using SambhramaBook.Application.Models.VendorAvailability;
using SambhramaBook.Application.Models.VendorBookings;
using SambhramaBook.Application.Models.VendorDashboard;
using SambhramaBook.Application.Models.VendorEarnings;
using SambhramaBook.Application.Models.VendorListings;
using SambhramaBook.Application.Models.VendorSettings;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/vendor")]
[Authorize]
public class VendorController : ControllerBase
{
    private readonly CompleteOnboardingHandler _completeOnboardingHandler;
    private readonly GetOnboardingStatusHandler _getOnboardingStatusHandler;
    private readonly IGetVendorBookingsHandler _getVendorBookingsHandler;
    private readonly ApproveBookingHandler _approveBookingHandler;
    private readonly RejectBookingHandler _rejectBookingHandler;
    private readonly CreateVendorBookingHandler _createVendorBookingHandler;
    private readonly IGetDashboardOverviewHandler _getDashboardOverviewHandler;
    private readonly IGetRecentBookingsHandler _getRecentBookingsHandler;
    private readonly IGetListingsSummaryHandler _getListingsSummaryHandler;
    private readonly IGetVendorListingsHandler _getVendorListingsHandler;
    private readonly IGetVendorListingDetailsHandler _getVendorListingDetailsHandler;
    private readonly CreateVendorListingHandler _createVendorListingHandler;
    private readonly UpdateVendorListingHandler _updateVendorListingHandler;
    private readonly DeleteVendorListingHandler _deleteVendorListingHandler;
    private readonly UpdateListingStatusHandler _updateListingStatusHandler;
    private readonly UploadListingImagesHandler _uploadListingImagesHandler;
    private readonly IGetVendorAvailabilityHandler _getVendorAvailabilityHandler;
    private readonly UpdateVendorAvailabilityHandler _updateVendorAvailabilityHandler;
    private readonly BlockDatesHandler _blockDatesHandler;
    private readonly UnblockDatesHandler _unblockDatesHandler;
    private readonly IGetEarningsSummaryHandler _getEarningsSummaryHandler;
    private readonly IGetEarningsTransactionsHandler _getEarningsTransactionsHandler;
    private readonly IGetPayoutHistoryHandler _getPayoutHistoryHandler;
    private readonly IGetVendorSettingsHandler _getVendorSettingsHandler;
    private readonly UpdateVendorSettingsHandler _updateVendorSettingsHandler;
    private readonly ILogger<VendorController> _logger;

    public VendorController(
        CompleteOnboardingHandler completeOnboardingHandler,
        GetOnboardingStatusHandler getOnboardingStatusHandler,
        IGetVendorBookingsHandler getVendorBookingsHandler,
        ApproveBookingHandler approveBookingHandler,
        RejectBookingHandler rejectBookingHandler,
        CreateVendorBookingHandler createVendorBookingHandler,
        IGetDashboardOverviewHandler getDashboardOverviewHandler,
        IGetRecentBookingsHandler getRecentBookingsHandler,
        IGetListingsSummaryHandler getListingsSummaryHandler,
        IGetVendorListingsHandler getVendorListingsHandler,
        IGetVendorListingDetailsHandler getVendorListingDetailsHandler,
        CreateVendorListingHandler createVendorListingHandler,
        UpdateVendorListingHandler updateVendorListingHandler,
        DeleteVendorListingHandler deleteVendorListingHandler,
        UpdateListingStatusHandler updateListingStatusHandler,
        UploadListingImagesHandler uploadListingImagesHandler,
        IGetVendorAvailabilityHandler getVendorAvailabilityHandler,
        UpdateVendorAvailabilityHandler updateVendorAvailabilityHandler,
        BlockDatesHandler blockDatesHandler,
        UnblockDatesHandler unblockDatesHandler,
        IGetEarningsSummaryHandler getEarningsSummaryHandler,
        IGetEarningsTransactionsHandler getEarningsTransactionsHandler,
        IGetPayoutHistoryHandler getPayoutHistoryHandler,
        IGetVendorSettingsHandler getVendorSettingsHandler,
        UpdateVendorSettingsHandler updateVendorSettingsHandler,
        ILogger<VendorController> logger)
    {
        _completeOnboardingHandler = completeOnboardingHandler;
        _getOnboardingStatusHandler = getOnboardingStatusHandler;
        _getVendorBookingsHandler = getVendorBookingsHandler;
        _approveBookingHandler = approveBookingHandler;
        _rejectBookingHandler = rejectBookingHandler;
        _createVendorBookingHandler = createVendorBookingHandler;
        _getDashboardOverviewHandler = getDashboardOverviewHandler;
        _getRecentBookingsHandler = getRecentBookingsHandler;
        _getListingsSummaryHandler = getListingsSummaryHandler;
        _getVendorListingsHandler = getVendorListingsHandler;
        _getVendorListingDetailsHandler = getVendorListingDetailsHandler;
        _createVendorListingHandler = createVendorListingHandler;
        _updateVendorListingHandler = updateVendorListingHandler;
        _deleteVendorListingHandler = deleteVendorListingHandler;
        _updateListingStatusHandler = updateListingStatusHandler;
        _uploadListingImagesHandler = uploadListingImagesHandler;
        _getVendorAvailabilityHandler = getVendorAvailabilityHandler;
        _updateVendorAvailabilityHandler = updateVendorAvailabilityHandler;
        _blockDatesHandler = blockDatesHandler;
        _unblockDatesHandler = unblockDatesHandler;
        _getEarningsSummaryHandler = getEarningsSummaryHandler;
        _getEarningsTransactionsHandler = getEarningsTransactionsHandler;
        _getPayoutHistoryHandler = getPayoutHistoryHandler;
        _getVendorSettingsHandler = getVendorSettingsHandler;
        _updateVendorSettingsHandler = updateVendorSettingsHandler;
        _logger = logger;
    }

    [HttpPost("onboarding")]
    [ProducesResponseType(typeof(CompleteOnboardingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CompleteOnboardingResponse>> CompleteOnboarding(
        [FromBody] CompleteOnboardingRequest request, 
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _completeOnboardingHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing onboarding");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("onboarding/status")]
    [ProducesResponseType(typeof(GetOnboardingStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetOnboardingStatusResponse>> GetOnboardingStatus(CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _getOnboardingStatusHandler.HandleAsync(userId, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting onboarding status");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("onboarding")]
    [ProducesResponseType(typeof(CompleteOnboardingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CompleteOnboardingResponse>> UpdateOnboarding(
        [FromBody] CompleteOnboardingRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _completeOnboardingHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating onboarding");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("bookings")]
    [ProducesResponseType(typeof(GetVendorBookingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetVendorBookingsResponse>> GetVendorBookings(
        [FromQuery] string status = "all",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var request = new GetVendorBookingsRequest
            {
                Status = status,
                Page = page,
                PageSize = pageSize
            };
            var response = await _getVendorBookingsHandler.Handle(userId, request, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vendor bookings");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("bookings/{id}/approve")]
    [ProducesResponseType(typeof(ApproveBookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApproveBookingResponse>> ApproveBooking(
        long id,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _approveBookingHandler.HandleAsync(userId, id, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving booking");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("bookings/{id}/reject")]
    [ProducesResponseType(typeof(RejectBookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RejectBookingResponse>> RejectBooking(
        long id,
        [FromBody] RejectBookingRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _rejectBookingHandler.HandleAsync(userId, id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting booking");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("bookings")]
    [ProducesResponseType(typeof(CreateVendorBookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateVendorBookingResponse>> CreateVendorBooking(
        [FromBody] CreateVendorBookingRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _createVendorBookingHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vendor booking");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("dashboard/overview")]
    [ProducesResponseType(typeof(DashboardOverviewResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardOverviewResponse>> GetDashboardOverview(CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _getDashboardOverviewHandler.Handle(userId, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard overview");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("dashboard/recent-bookings")]
    [ProducesResponseType(typeof(GetRecentBookingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetRecentBookingsResponse>> GetRecentBookings(
        [FromQuery] int limit = 5,
        CancellationToken ct = default)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _getRecentBookingsHandler.Handle(userId, limit, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recent bookings");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("dashboard/listings")]
    [ProducesResponseType(typeof(GetListingsSummaryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetListingsSummaryResponse>> GetListingsSummary(CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _getListingsSummaryHandler.Handle(userId, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting listings summary");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("listings")]
    [ProducesResponseType(typeof(GetVendorListingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetVendorListingsResponse>> GetVendorListings(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var request = new GetVendorListingsRequest { Page = page, PageSize = pageSize };
            var response = await _getVendorListingsHandler.Handle(userId, request, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vendor listings");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("listings/{id}")]
    [ProducesResponseType(typeof(ApiResponse<VendorListingDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetVendorListingDetails(long id, CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var listing = await _getVendorListingDetailsHandler.Handle(userId, id, ct);
            if (listing == null)
            {
                return NotFound(new { success = false, message = "Listing not found" });
            }
            return Ok(new { success = true, data = listing });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vendor listing details");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPost("listings")]
    [ProducesResponseType(typeof(CreateVendorListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateVendorListingResponse>> CreateVendorListing(
        [FromBody] CreateVendorListingRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _createVendorListingHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vendor listing");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("listings/{id}")]
    [ProducesResponseType(typeof(UpdateVendorListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateVendorListingResponse>> UpdateVendorListing(
        long id,
        [FromBody] CreateVendorListingRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updateVendorListingHandler.HandleAsync(userId, id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vendor listing");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpDelete("listings/{id}")]
    [ProducesResponseType(typeof(DeleteVendorListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteVendorListingResponse>> DeleteVendorListing(long id, CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _deleteVendorListingHandler.HandleAsync(userId, id, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting vendor listing");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("listings/{id}/status")]
    [ProducesResponseType(typeof(UpdateListingStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateListingStatusResponse>> UpdateListingStatus(
        long id,
        [FromBody] UpdateListingStatusRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updateListingStatusHandler.HandleAsync(userId, id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating listing status");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("listings/{id}/images")]
    [ProducesResponseType(typeof(UploadListingImagesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UploadListingImagesResponse>> UploadListingImages(
        long id,
        [FromForm] List<IFormFile> images,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            // TODO: Upload files to storage (S3, Azure Blob, etc.) and get URLs
            // For now, return placeholder URLs
            var imageUrls = new List<string>();
            foreach (var image in images)
            {
                // In production, upload to storage and get URL
                imageUrls.Add($"https://example.com/uploaded-{Guid.NewGuid()}.jpg");
            }

            var response = await _uploadListingImagesHandler.HandleAsync(userId, id, imageUrls, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading listing images");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("availability/{listingId}")]
    [ProducesResponseType(typeof(GetVendorAvailabilityResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetVendorAvailabilityResponse>> GetVendorAvailability(
        long listingId,
        [FromQuery] string month,
        [FromQuery] int year,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var request = new GetVendorAvailabilityRequest
            {
                ListingId = listingId,
                Month = month,
                Year = year
            };
            var response = await _getVendorAvailabilityHandler.Handle(userId, request, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vendor availability");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("availability/{listingId}")]
    [ProducesResponseType(typeof(UpdateVendorAvailabilityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateVendorAvailabilityResponse>> UpdateVendorAvailability(
        long listingId,
        [FromBody] UpdateVendorAvailabilityRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updateVendorAvailabilityHandler.HandleAsync(userId, listingId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vendor availability");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("availability/{listingId}/block")]
    [ProducesResponseType(typeof(BlockDatesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BlockDatesResponse>> BlockDates(
        long listingId,
        [FromBody] BlockDatesRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _blockDatesHandler.HandleAsync(userId, listingId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error blocking dates");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpDelete("availability/{listingId}/unblock")]
    [ProducesResponseType(typeof(UnblockDatesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UnblockDatesResponse>> UnblockDates(
        long listingId,
        [FromBody] UnblockDatesRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _unblockDatesHandler.HandleAsync(userId, listingId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unblocking dates");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("earnings")]
    [ProducesResponseType(typeof(GetEarningsSummaryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetEarningsSummaryResponse>> GetEarningsSummary(
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var request = new GetEarningsSummaryRequest { StartDate = startDate, EndDate = endDate };
            var response = await _getEarningsSummaryHandler.Handle(userId, request, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting earnings summary");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("earnings/transactions")]
    [ProducesResponseType(typeof(GetEarningsTransactionsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetEarningsTransactionsResponse>> GetEarningsTransactions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var request = new GetEarningsTransactionsRequest { Page = page, PageSize = pageSize };
            var response = await _getEarningsTransactionsHandler.Handle(userId, request, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting earnings transactions");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("earnings/payouts")]
    [ProducesResponseType(typeof(GetPayoutHistoryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetPayoutHistoryResponse>> GetPayoutHistory(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var request = new GetPayoutHistoryRequest { Page = page, PageSize = pageSize };
            var response = await _getPayoutHistoryHandler.Handle(userId, request, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payout history");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("settings")]
    [ProducesResponseType(typeof(VendorSettingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<VendorSettingsResponse>> GetVendorSettings(CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _getVendorSettingsHandler.Handle(userId, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vendor settings");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("settings")]
    [ProducesResponseType(typeof(UpdateVendorSettingsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateVendorSettingsResponse>> UpdateVendorSettings(
        [FromBody] UpdateVendorSettingsRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updateVendorSettingsHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vendor settings");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}

