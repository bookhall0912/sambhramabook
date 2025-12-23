using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Common;
using SambhramaBook.Application.Handlers.Payment;
using SambhramaBook.Application.Models.Payment;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly InitiatePaymentHandler _initiatePaymentHandler;
    private readonly VerifyPaymentHandler _verifyPaymentHandler;
    private readonly ProcessRefundHandler _processRefundHandler;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(
        InitiatePaymentHandler initiatePaymentHandler,
        VerifyPaymentHandler verifyPaymentHandler,
        ProcessRefundHandler processRefundHandler,
        ILogger<PaymentsController> logger)
    {
        _initiatePaymentHandler = initiatePaymentHandler;
        _verifyPaymentHandler = verifyPaymentHandler;
        _processRefundHandler = processRefundHandler;
        _logger = logger;
    }

    [HttpPost("initiate")]
    [Authorize]
    [ProducesResponseType(typeof(InitiatePaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InitiatePaymentResponse>> InitiatePayment(
        [FromBody] InitiatePaymentRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _initiatePaymentHandler.HandleAsync(userId, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating payment");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("verify")]
    [ProducesResponseType(typeof(VerifyPaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VerifyPaymentResponse>> VerifyPayment(
        [FromBody] VerifyPaymentRequest request,
        CancellationToken ct)
    {
        try
        {
            var response = await _verifyPaymentHandler.HandleAsync(request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying payment");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("{id}/refund")]
    [Authorize]
    [ProducesResponseType(typeof(ProcessRefundResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProcessRefundResponse>> ProcessRefund(
        long id,
        [FromBody] ProcessRefundRequest request,
        CancellationToken ct)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { success = false, message = "Invalid user" });
            }

            var response = await _processRefundHandler.HandleAsync(userId, id, request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing refund");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}

