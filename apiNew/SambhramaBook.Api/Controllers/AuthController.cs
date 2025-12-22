using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Handlers.Auth;
using SambhramaBook.Application.Models.Auth;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SendOtpHandler _sendOtpHandler;
    private readonly VerifyOtpHandler _verifyOtpHandler;
    private readonly GetCurrentUserHandler _getCurrentUserHandler;
    private readonly LogoutHandler _logoutHandler;
    private readonly RefreshTokenHandler _refreshTokenHandler;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        SendOtpHandler sendOtpHandler,
        VerifyOtpHandler verifyOtpHandler,
        GetCurrentUserHandler getCurrentUserHandler,
        LogoutHandler logoutHandler,
        RefreshTokenHandler refreshTokenHandler,
        ILogger<AuthController> logger)
    {
        _sendOtpHandler = sendOtpHandler;
        _verifyOtpHandler = verifyOtpHandler;
        _getCurrentUserHandler = getCurrentUserHandler;
        _logoutHandler = logoutHandler;
        _refreshTokenHandler = refreshTokenHandler;
        _logger = logger;
    }

    [HttpPost("send-otp")]
    [ProducesResponseType(typeof(SendOtpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SendOtpResponse>> SendOtp([FromBody] SendOtpRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _sendOtpHandler.HandleAsync(request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending OTP");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("verify-otp")]
    [ProducesResponseType(typeof(VerifyOtpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VerifyOtpResponse>> VerifyOtp([FromBody] VerifyOtpRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _verifyOtpHandler.HandleAsync(request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying OTP");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(GetCurrentUserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetCurrentUserResponse>> GetCurrentUser(CancellationToken ct)
    {
        try
        {
            var response = await _getCurrentUserHandler.HandleAsync(ct);
            if (response == null)
            {
                return Unauthorized(new { success = false, message = "User not found" });
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(LogoutResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LogoutResponse>> Logout(CancellationToken ct)
    {
        try
        {
            var response = await _logoutHandler.HandleAsync(ct);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging out");
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _refreshTokenHandler.HandleAsync(request, ct);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}
