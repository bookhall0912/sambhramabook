using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Handlers.Admin.Analytics;
using SambhramaBook.Application.Handlers.Admin.Bookings;
using SambhramaBook.Application.Handlers.Admin.Dashboard;
using SambhramaBook.Application.Handlers.Admin.Listings;
using SambhramaBook.Application.Handlers.Admin.Payouts;
using SambhramaBook.Application.Handlers.Admin.Reviews;
using SambhramaBook.Application.Handlers.Admin.Settings;
using SambhramaBook.Application.Handlers.Admin.Users;
using SambhramaBook.Application.Handlers.Admin.Vendors;
using SambhramaBook.Application.Handlers.Booking;
using SambhramaBook.Application.Models.Admin;
using SambhramaBook.Application.Models.Booking;
using SambhramaBook.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using SambhramaBook.Application.Handlers.Admin.Reports;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IGetAdminDashboardOverviewHandler _getAdminDashboardOverviewHandler;
    private readonly IGetPendingListingsHandler _getPendingListingsHandler;
    private readonly IGetPendingPayoutsHandler _getPendingPayoutsHandler;
    private readonly IGetAnalyticsSummaryHandler _getAnalyticsSummaryHandler;
    private readonly IGetRevenueAnalyticsHandler _getRevenueAnalyticsHandler;
    private readonly IGetUserAnalyticsHandler _getUserAnalyticsHandler;
    private readonly IGetBookingAnalyticsHandler _getBookingAnalyticsHandler;
    private readonly IGetAllUsersHandler _getAllUsersHandler;
    private readonly IGetUserDetailsHandler _getUserDetailsHandler;
    private readonly UpdateUserStatusHandler _updateUserStatusHandler;
    private readonly DeleteUserHandler _deleteUserHandler;
    private readonly IGetAllVendorsHandler _getAllVendorsHandler;
    private readonly IGetVendorDetailsHandler _getVendorDetailsHandler;
    private readonly VerifyVendorHandler _verifyVendorHandler;
    private readonly UpdateVendorStatusHandler _updateVendorStatusHandler;
    private readonly IGetPendingListingsForApprovalHandler _getPendingListingsForApprovalHandler;
    private readonly IListingRepository _listingRepository;
    private readonly ApproveListingHandler _approveListingHandler;
    private readonly RejectListingHandler _rejectListingHandler;
    private readonly RequestListingChangesHandler _requestListingChangesHandler;
    private readonly IGetAllBookingsHandler _getAllBookingsHandler;
    private readonly IGetBookingByIdHandler _getBookingByIdHandler;
    private readonly UpdateBookingStatusHandler _updateBookingStatusHandler;
    private readonly IGetAllPayoutsHandler _getAllPayoutsHandler;
    private readonly ProcessPayoutHandler _processPayoutHandler;
    private readonly UpdatePayoutStatusHandler _updatePayoutStatusHandler;
    private readonly IGetRevenueReportHandler _getRevenueReportHandler;
    private readonly IGetBookingReportHandler _getBookingReportHandler;
    private readonly IGetVendorReportHandler _getVendorReportHandler;
    private readonly IGetUserReportHandler _getUserReportHandler;
    private readonly IGetAllReviewsHandler _getAllReviewsHandler;
    private readonly PublishReviewHandler _publishReviewHandler;
    private readonly DeleteReviewHandler _deleteReviewHandler;
    private readonly IGetAllSettingsHandler _getAllSettingsHandler;
    private readonly IGetSettingHandler _getSettingHandler;
    private readonly UpdateSettingHandler _updateSettingHandler;
    private readonly CreateSettingHandler _createSettingHandler;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        IGetAdminDashboardOverviewHandler getAdminDashboardOverviewHandler,
        IGetPendingListingsHandler getPendingListingsHandler,
        IGetPendingPayoutsHandler getPendingPayoutsHandler,
        IGetAnalyticsSummaryHandler getAnalyticsSummaryHandler,
        IGetRevenueAnalyticsHandler getRevenueAnalyticsHandler,
        IGetUserAnalyticsHandler getUserAnalyticsHandler,
        IGetBookingAnalyticsHandler getBookingAnalyticsHandler,
        IGetAllUsersHandler getAllUsersHandler,
        IGetUserDetailsHandler getUserDetailsHandler,
        UpdateUserStatusHandler updateUserStatusHandler,
        DeleteUserHandler deleteUserHandler,
        IGetAllVendorsHandler getAllVendorsHandler,
        IGetVendorDetailsHandler getVendorDetailsHandler,
        VerifyVendorHandler verifyVendorHandler,
        UpdateVendorStatusHandler updateVendorStatusHandler,
        IGetPendingListingsForApprovalHandler getPendingListingsForApprovalHandler,
        IListingRepository listingRepository,
        ApproveListingHandler approveListingHandler,
        RejectListingHandler rejectListingHandler,
        RequestListingChangesHandler requestListingChangesHandler,
        IGetAllBookingsHandler getAllBookingsHandler,
        IGetBookingByIdHandler getBookingByIdHandler,
        UpdateBookingStatusHandler updateBookingStatusHandler,
        IGetAllPayoutsHandler getAllPayoutsHandler,
        ProcessPayoutHandler processPayoutHandler,
        UpdatePayoutStatusHandler updatePayoutStatusHandler,
        IGetRevenueReportHandler getRevenueReportHandler,
        IGetBookingReportHandler getBookingReportHandler,
        IGetVendorReportHandler getVendorReportHandler,
        IGetUserReportHandler getUserReportHandler,
        IGetAllReviewsHandler getAllReviewsHandler,
        PublishReviewHandler publishReviewHandler,
        DeleteReviewHandler deleteReviewHandler,
        IGetAllSettingsHandler getAllSettingsHandler,
        IGetSettingHandler getSettingHandler,
        UpdateSettingHandler updateSettingHandler,
        CreateSettingHandler createSettingHandler,
        ILogger<AdminController> logger)
    {
        _getAdminDashboardOverviewHandler = getAdminDashboardOverviewHandler;
        _getPendingListingsHandler = getPendingListingsHandler;
        _getPendingPayoutsHandler = getPendingPayoutsHandler;
        _getAnalyticsSummaryHandler = getAnalyticsSummaryHandler;
        _getRevenueAnalyticsHandler = getRevenueAnalyticsHandler;
        _getUserAnalyticsHandler = getUserAnalyticsHandler;
        _getBookingAnalyticsHandler = getBookingAnalyticsHandler;
        _getAllUsersHandler = getAllUsersHandler;
        _getUserDetailsHandler = getUserDetailsHandler;
        _updateUserStatusHandler = updateUserStatusHandler;
        _deleteUserHandler = deleteUserHandler;
        _getAllVendorsHandler = getAllVendorsHandler;
        _getVendorDetailsHandler = getVendorDetailsHandler;
        _verifyVendorHandler = verifyVendorHandler;
        _updateVendorStatusHandler = updateVendorStatusHandler;
        _getPendingListingsForApprovalHandler = getPendingListingsForApprovalHandler;
        _listingRepository = listingRepository;
        _approveListingHandler = approveListingHandler;
        _rejectListingHandler = rejectListingHandler;
        _requestListingChangesHandler = requestListingChangesHandler;
        _getAllBookingsHandler = getAllBookingsHandler;
        _getBookingByIdHandler = getBookingByIdHandler;
        _updateBookingStatusHandler = updateBookingStatusHandler;
        _getAllPayoutsHandler = getAllPayoutsHandler;
        _processPayoutHandler = processPayoutHandler;
        _updatePayoutStatusHandler = updatePayoutStatusHandler;
        _getRevenueReportHandler = getRevenueReportHandler;
        _getBookingReportHandler = getBookingReportHandler;
        _getVendorReportHandler = getVendorReportHandler;
        _getUserReportHandler = getUserReportHandler;
        _getAllReviewsHandler = getAllReviewsHandler;
        _publishReviewHandler = publishReviewHandler;
        _deleteReviewHandler = deleteReviewHandler;
        _getAllSettingsHandler = getAllSettingsHandler;
        _getSettingHandler = getSettingHandler;
        _updateSettingHandler = updateSettingHandler;
        _createSettingHandler = createSettingHandler;
        _logger = logger;
    }

    private long? GetAdminUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
        {
            return null;
        }
        return userId;
    }

    // Dashboard APIs
    [HttpGet("dashboard/overview")]
    [ProducesResponseType(typeof(AdminDashboardOverviewResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AdminDashboardOverviewResponse>> GetDashboardOverview(CancellationToken ct)
    {
        try
        {
            var response = await _getAdminDashboardOverviewHandler.Handle(ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting admin dashboard overview");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("dashboard/pending-listings")]
    [ProducesResponseType(typeof(GetPendingListingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetPendingListingsResponse>> GetPendingListings(
        [FromQuery] int limit = 5,
        CancellationToken ct = default)
    {
        try
        {
            var response = await _getPendingListingsHandler.Handle(limit, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending listings");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("dashboard/pending-payouts")]
    [ProducesResponseType(typeof(GetPendingPayoutsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetPendingPayoutsResponse>> GetPendingPayouts(
        [FromQuery] int limit = 5,
        CancellationToken ct = default)
    {
        try
        {
            var response = await _getPendingPayoutsHandler.Handle(limit, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending payouts");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    // Analytics APIs
    [HttpGet("analytics/summary")]
    [ProducesResponseType(typeof(GetAnalyticsSummaryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAnalyticsSummaryResponse>> GetAnalyticsSummary(
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetAnalyticsSummaryRequest { StartDate = startDate, EndDate = endDate };
            var response = await _getAnalyticsSummaryHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting analytics summary");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("analytics/revenue")]
    [ProducesResponseType(typeof(GetRevenueAnalyticsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetRevenueAnalyticsResponse>> GetRevenueAnalytics(
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromQuery] string period = "monthly",
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetRevenueAnalyticsRequest { Period = period, StartDate = startDate, EndDate = endDate };
            var response = await _getRevenueAnalyticsHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting revenue analytics");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("analytics/users")]
    [ProducesResponseType(typeof(GetUserAnalyticsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetUserAnalyticsResponse>> GetUserAnalytics(
        [FromQuery] string period = "monthly",
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetUserAnalyticsRequest { Period = period };
            var response = await _getUserAnalyticsHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user analytics");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("analytics/bookings")]
    [ProducesResponseType(typeof(GetBookingAnalyticsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetBookingAnalyticsResponse>> GetBookingAnalytics(
        [FromQuery] string period = "monthly",
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetBookingAnalyticsRequest { Period = period };
            var response = await _getBookingAnalyticsHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking analytics");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    // Users APIs
    [HttpGet("users")]
    [ProducesResponseType(typeof(GetAllUsersResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllUsersResponse>> GetAllUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetAllUsersRequest { Page = page, PageSize = pageSize, Search = search, Status = status };
            var response = await _getAllUsersHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("users/{id}")]
    [ProducesResponseType(typeof(ApiResponse<AdminUserDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUserDetails(long id, CancellationToken ct)
    {
        try
        {
            var user = await _getUserDetailsHandler.Handle(id, ct);
            if (user == null)
            {
                return NotFound(new { success = false, message = "User not found" });
            }
            return Ok(new { success = true, data = user });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user details");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("users/{id}/status")]
    [ProducesResponseType(typeof(UpdateUserStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateUserStatusResponse>> UpdateUserStatus(
        long id,
        [FromBody] UpdateUserStatusRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updateUserStatusHandler.HandleAsync(id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user status");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpDelete("users/{id}")]
    [ProducesResponseType(typeof(DeleteUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteUserResponse>> DeleteUser(long id, CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _deleteUserHandler.HandleAsync(id, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // Vendors APIs
    [HttpGet("vendors")]
    [ProducesResponseType(typeof(GetAllVendorsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllVendorsResponse>> GetAllVendors(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetAllVendorsRequest { Page = page, PageSize = pageSize, Status = status };
            var response = await _getAllVendorsHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all vendors");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("vendors/{id}")]
    [ProducesResponseType(typeof(ApiResponse<AdminVendorDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetVendorDetails(long id, CancellationToken ct)
    {
        try
        {
            var vendor = await _getVendorDetailsHandler.Handle(id, ct);
            if (vendor == null)
            {
                return NotFound(new { success = false, message = "Vendor not found" });
            }
            return Ok(new { success = true, data = vendor });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vendor details");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("vendors/{id}/verify")]
    [ProducesResponseType(typeof(VerifyVendorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VerifyVendorResponse>> VerifyVendor(
        long id,
        [FromBody] VerifyVendorRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _verifyVendorHandler.HandleAsync(id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying vendor");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("vendors/{id}/status")]
    [ProducesResponseType(typeof(UpdateVendorStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateVendorStatusResponse>> UpdateVendorStatus(
        long id,
        [FromBody] UpdateVendorStatusRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updateVendorStatusHandler.HandleAsync(id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vendor status");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // Listings Approval APIs
    [HttpGet("listings/pending")]
    [ProducesResponseType(typeof(GetPendingListingsForApprovalResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetPendingListingsForApprovalResponse>> GetPendingListingsForApproval(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetPendingListingsForApprovalRequest { Page = page, PageSize = pageSize };
            var response = await _getPendingListingsForApprovalHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending listings for approval");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("listings/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetListingDetails(long id, CancellationToken ct)
    {
        try
        {
            var listing = await _listingRepository.GetByIdAsync(id, ct);
            if (listing == null)
            {
                return NotFound(new { success = false, message = "Listing not found" });
            }
            // Return listing details - can be enhanced with more details if needed
            return Ok(new { success = true, data = listing });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting listing details");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("listings/{id}/approve")]
    [ProducesResponseType(typeof(ApproveListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApproveListingResponse>> ApproveListing(long id, CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _approveListingHandler.HandleAsync(id, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving listing");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("listings/{id}/reject")]
    [ProducesResponseType(typeof(RejectListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RejectListingResponse>> RejectListing(
        long id,
        [FromBody] RejectListingRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _rejectListingHandler.HandleAsync(id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting listing");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("listings/{id}/needs-changes")]
    [ProducesResponseType(typeof(RequestListingChangesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RequestListingChangesResponse>> RequestListingChanges(
        long id,
        [FromBody] RequestListingChangesRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _requestListingChangesHandler.HandleAsync(id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting listing changes");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // Bookings APIs
    [HttpGet("bookings")]
    [ProducesResponseType(typeof(GetAllBookingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllBookingsResponse>> GetAllBookings(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] DateOnly? endDate = null,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetAllBookingsRequest
            {
                Page = page,
                PageSize = pageSize,
                Status = status,
                StartDate = startDate,
                EndDate = endDate
            };
            var response = await _getAllBookingsHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all bookings");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("bookings/{id}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetBookingDetails(long id, CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var booking = await _getBookingByIdHandler.Handle(adminUserId!.Value, id, ct);
            if (booking == null)
            {
                return NotFound(new { success = false, message = "Booking not found" });
            }
            return Ok(new { success = true, data = booking });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking details");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("bookings/{id}/status")]
    [ProducesResponseType(typeof(UpdateBookingStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateBookingStatusResponse>> UpdateBookingStatus(
        long id,
        [FromBody] UpdateBookingStatusRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updateBookingStatusHandler.HandleAsync(id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking status");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // Payouts APIs
    [HttpGet("payouts")]
    [ProducesResponseType(typeof(GetAllPayoutsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllPayoutsResponse>> GetAllPayouts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetAllPayoutsRequest { Page = page, PageSize = pageSize, Status = status };
            var response = await _getAllPayoutsHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all payouts");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("payouts/pending")]
    [ProducesResponseType(typeof(GetAllPayoutsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllPayoutsResponse>> GetPendingPayouts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetAllPayoutsRequest { Page = page, PageSize = pageSize, Status = "PENDING" };
            var response = await _getAllPayoutsHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending payouts");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPost("payouts/{id}/process")]
    [ProducesResponseType(typeof(ProcessPayoutResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProcessPayoutResponse>> ProcessPayout(long id, CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _processPayoutHandler.HandleAsync(id, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payout");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("payouts/{id}/status")]
    [ProducesResponseType(typeof(UpdatePayoutStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdatePayoutStatusResponse>> UpdatePayoutStatus(
        long id,
        [FromBody] UpdatePayoutStatusRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updatePayoutStatusHandler.HandleAsync(id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payout status");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // Reports APIs
    [HttpGet("reports/revenue")]
    [ProducesResponseType(typeof(GetRevenueReportResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetRevenueReportResponse>> GetRevenueReport(
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromQuery] string? format = "json",
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetRevenueReportRequest { StartDate = startDate, EndDate = endDate, Format = format };
            var response = await _getRevenueReportHandler.Handle(request, ct);
            // TODO: Handle CSV/Excel export if format is specified
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting revenue report");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("reports/bookings")]
    [ProducesResponseType(typeof(GetBookingReportResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetBookingReportResponse>> GetBookingReport(
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromQuery] string? format = "json",
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetBookingReportRequest { StartDate = startDate, EndDate = endDate, Format = format };
            var response = await _getBookingReportHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking report");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("reports/vendors")]
    [ProducesResponseType(typeof(GetVendorReportResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetVendorReportResponse>> GetVendorReport(
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromQuery] string? format = "json",
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetVendorReportRequest { StartDate = startDate, EndDate = endDate, Format = format };
            var response = await _getVendorReportHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vendor report");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("reports/users")]
    [ProducesResponseType(typeof(GetUserReportResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetUserReportResponse>> GetUserReport(
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromQuery] string? format = "json",
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetUserReportRequest { StartDate = startDate, EndDate = endDate, Format = format };
            var response = await _getUserReportHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user report");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("reports/export")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ExportReport(
        [FromQuery] string type,
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        [FromQuery] string format = "csv",
        CancellationToken ct = default)
    {
        try
        {
            // TODO: Implement CSV/Excel export
            return BadRequest(new { success = false, message = "Export functionality not yet implemented" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting report");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    // Reviews APIs
    [HttpGet("reviews")]
    [ProducesResponseType(typeof(GetAllReviewsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllReviewsResponse>> GetAllReviews(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        try
        {
            var request = new GetAllReviewsRequest { Page = page, PageSize = pageSize, Status = status };
            var response = await _getAllReviewsHandler.Handle(request, ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all reviews");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("reviews/{id}/publish")]
    [ProducesResponseType(typeof(PublishReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PublishReviewResponse>> PublishReview(
        long id,
        [FromBody] PublishReviewRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _publishReviewHandler.HandleAsync(id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing review");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpDelete("reviews/{id}")]
    [ProducesResponseType(typeof(DeleteReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DeleteReviewResponse>> DeleteReview(long id, CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _deleteReviewHandler.HandleAsync(id, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting review");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // Platform Settings APIs
    [HttpGet("settings")]
    [ProducesResponseType(typeof(GetAllSettingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetAllSettingsResponse>> GetAllSettings(CancellationToken ct)
    {
        try
        {
            var response = await _getAllSettingsHandler.Handle(ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all settings");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("settings/{key}")]
    [ProducesResponseType(typeof(GetSettingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetSettingResponse>> GetSetting(string key, CancellationToken ct)
    {
        try
        {
            var response = await _getSettingHandler.Handle(key, ct);
            if (response == null)
            {
                return NotFound(new { success = false, message = "Setting not found" });
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting setting");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("settings/{key}")]
    [ProducesResponseType(typeof(UpdateSettingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateSettingResponse>> UpdateSetting(
        string key,
        [FromBody] UpdateSettingRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updateSettingHandler.HandleAsync(key, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating setting");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("settings")]
    [ProducesResponseType(typeof(CreateSettingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateSettingResponse>> CreateSetting(
        [FromBody] CreateSettingRequest request,
        CancellationToken ct)
    {
        try
        {
            var adminUserId = GetAdminUserId();
            if (!adminUserId.HasValue)
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _createSettingHandler.HandleAsync(request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating setting");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}

