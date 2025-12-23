using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Handlers.Booking;
using SambhramaBook.Application.Handlers.Notifications;
using SambhramaBook.Application.Handlers.SavedVenues;
using SambhramaBook.Application.Handlers.UserBookings;
using SambhramaBook.Application.Handlers.UserProfile;
using SambhramaBook.Application.Models.Booking;
using SambhramaBook.Application.Models.Notifications;
using SambhramaBook.Application.Models.SavedVenues;
using SambhramaBook.Application.Models.UserProfile;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IGetUserBookingsHandler _getUserBookingsHandler;
    private readonly IGetBookingByIdHandler _getBookingByIdHandler;
    private readonly IGetUserProfileHandler _getUserProfileHandler;
    private readonly UpdateUserProfileHandler _updateUserProfileHandler;
    private readonly ChangePasswordHandler _changePasswordHandler;
    private readonly IGetSavedListingsHandler _getSavedListingsHandler;
    private readonly SaveListingHandler _saveListingHandler;
    private readonly RemoveSavedListingHandler _removeSavedListingHandler;
    private readonly IGetNotificationsHandler _getNotificationsHandler;
    private readonly MarkNotificationReadHandler _markNotificationReadHandler;
    private readonly MarkAllNotificationsReadHandler _markAllNotificationsReadHandler;
    private readonly IGetUnreadCountHandler _getUnreadCountHandler;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IGetUserBookingsHandler getUserBookingsHandler,
        IGetBookingByIdHandler getBookingByIdHandler,
        IGetUserProfileHandler getUserProfileHandler,
        UpdateUserProfileHandler updateUserProfileHandler,
        ChangePasswordHandler changePasswordHandler,
        IGetSavedListingsHandler getSavedListingsHandler,
        SaveListingHandler saveListingHandler,
        RemoveSavedListingHandler removeSavedListingHandler,
        IGetNotificationsHandler getNotificationsHandler,
        MarkNotificationReadHandler markNotificationReadHandler,
        MarkAllNotificationsReadHandler markAllNotificationsReadHandler,
        IGetUnreadCountHandler getUnreadCountHandler,
        ILogger<UserController> logger)
    {
        _getUserBookingsHandler = getUserBookingsHandler;
        _getBookingByIdHandler = getBookingByIdHandler;
        _getUserProfileHandler = getUserProfileHandler;
        _updateUserProfileHandler = updateUserProfileHandler;
        _changePasswordHandler = changePasswordHandler;
        _getSavedListingsHandler = getSavedListingsHandler;
        _saveListingHandler = saveListingHandler;
        _removeSavedListingHandler = removeSavedListingHandler;
        _getNotificationsHandler = getNotificationsHandler;
        _markNotificationReadHandler = markNotificationReadHandler;
        _markAllNotificationsReadHandler = markAllNotificationsReadHandler;
        _getUnreadCountHandler = getUnreadCountHandler;
        _logger = logger;
    }

    [HttpGet("bookings")]
    [ProducesResponseType(typeof(GetUserBookingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetUserBookingsResponse>> GetUserBookings(
        [FromQuery] string? status,
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

            var request = new GetUserBookingsRequest
            {
                Status = status,
                Page = page,
                PageSize = pageSize
            };
            var response = await _getUserBookingsHandler.Handle(userId, request, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user bookings");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("bookings/{id}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUserBooking(long id, CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var booking = await _getBookingByIdHandler.Handle(userId, id, ct);
            if (booking == null)
            {
                return NotFound(new { success = false, message = "Booking not found" });
            }
            return Ok(new { success = true, data = booking });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user booking");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUserProfile(CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var profile = await _getUserProfileHandler.Handle(userId, ct);
            if (profile == null)
            {
                return NotFound(new { success = false, message = "User not found" });
            }
            return Ok(new { success = true, data = profile });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("profile")]
    [ProducesResponseType(typeof(UpdateUserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateUserProfileResponse>> UpdateUserProfile(
        [FromBody] UpdateUserProfileRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _updateUserProfileHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("profile/password")]
    [ProducesResponseType(typeof(ChangePasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ChangePasswordResponse>> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _changePasswordHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("saved")]
    [ProducesResponseType(typeof(GetSavedListingsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetSavedListingsResponse>> GetSavedListings(CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _getSavedListingsHandler.Handle(userId, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting saved listings");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPost("saved/{listingId}")]
    [ProducesResponseType(typeof(SaveListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SaveListingResponse>> SaveListing(long listingId, CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _saveListingHandler.HandleAsync(userId, listingId, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving listing");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpDelete("saved/{listingId}")]
    [ProducesResponseType(typeof(RemoveSavedListingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RemoveSavedListingResponse>> RemoveSavedListing(long listingId, CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _removeSavedListingHandler.HandleAsync(userId, listingId, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing saved listing");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("notifications")]
    [ProducesResponseType(typeof(GetNotificationsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GetNotificationsResponse>> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var request = new GetNotificationsRequest { Page = page, PageSize = pageSize };
            var response = await _getNotificationsHandler.Handle(userId, request, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notifications");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("notifications/{id}/read")]
    [ProducesResponseType(typeof(MarkNotificationReadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MarkNotificationReadResponse>> MarkNotificationRead(long id, CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _markNotificationReadHandler.HandleAsync(userId, id, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification as read");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("notifications/read-all")]
    [ProducesResponseType(typeof(MarkAllNotificationsReadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MarkAllNotificationsReadResponse>> MarkAllNotificationsRead(CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _markAllNotificationsReadHandler.HandleAsync(userId, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking all notifications as read");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("notifications/unread-count")]
    [ProducesResponseType(typeof(UnreadCountResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UnreadCountResponse>> GetUnreadCount(CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _getUnreadCountHandler.Handle(userId, ct);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unread count");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}

