using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SambhramaBook.Application.Handlers.Vendor;
using SambhramaBook.Application.Models.Vendor;

namespace SambhramaBook.Api.Controllers;

[ApiController]
[Route("api/vendor")]
[Authorize]
public class VendorController : ControllerBase
{
    private readonly CompleteOnboardingHandler _completeOnboardingHandler;
    private readonly ILogger<VendorController> _logger;

    public VendorController(
        CompleteOnboardingHandler completeOnboardingHandler,
        ILogger<VendorController> logger)
    {
        _completeOnboardingHandler = completeOnboardingHandler;
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
            var response = await _completeOnboardingHandler.HandleAsync(request, ct);
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
}

