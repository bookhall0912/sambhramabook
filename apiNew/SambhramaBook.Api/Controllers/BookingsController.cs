using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Handlers.Booking;
using SambhramaBook.Application.Models.Booking;
using SambhramaBook.Application.Models.Payment;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly CreateBookingHandler _createBookingHandler;
    private readonly IGetBookingByIdHandler _getBookingByIdHandler;
    private readonly IGetBookingByReferenceHandler _getBookingByReferenceHandler;
    private readonly CancelBookingHandler _cancelBookingHandler;
    private readonly RescheduleBookingHandler _rescheduleBookingHandler;
    private readonly ProcessPaymentHandler _processPaymentHandler;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(
        CreateBookingHandler createBookingHandler,
        IGetBookingByIdHandler getBookingByIdHandler,
        IGetBookingByReferenceHandler getBookingByReferenceHandler,
        CancelBookingHandler cancelBookingHandler,
        RescheduleBookingHandler rescheduleBookingHandler,
        ProcessPaymentHandler processPaymentHandler,
        ILogger<BookingsController> logger)
    {
        _createBookingHandler = createBookingHandler;
        _getBookingByIdHandler = getBookingByIdHandler;
        _getBookingByReferenceHandler = getBookingByReferenceHandler;
        _cancelBookingHandler = cancelBookingHandler;
        _rescheduleBookingHandler = rescheduleBookingHandler;
        _processPaymentHandler = processPaymentHandler;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateBookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateBookingResponse>> CreateBooking(
        [FromBody] CreateBookingRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _createBookingHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetBookingById(long id, CancellationToken ct)
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
            _logger.LogError(ex, "Error getting booking");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet("reference/{reference}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetBookingByReference(string reference, CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var booking = await _getBookingByReferenceHandler.Handle(userId, reference, ct);
            if (booking == null)
            {
                return NotFound(new { success = false, message = "Booking not found" });
            }
            return Ok(new { success = true, data = booking });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking by reference");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("{id}/cancel")]
    [ProducesResponseType(typeof(CancelBookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CancelBookingResponse>> CancelBooking(
        long id,
        [FromBody] CancelBookingRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _cancelBookingHandler.HandleAsync(userId, id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPut("{id}/reschedule")]
    [ProducesResponseType(typeof(RescheduleBookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RescheduleBookingResponse>> RescheduleBooking(
        long id,
        [FromBody] RescheduleBookingRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _rescheduleBookingHandler.HandleAsync(userId, id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rescheduling booking");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("{id}/payment")]
    [ProducesResponseType(typeof(ProcessPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProcessPaymentResponse>> ProcessPayment(
        long id,
        [FromBody] ProcessPaymentRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _processPaymentHandler.HandleAsync(userId, id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}

