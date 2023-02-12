using Microsoft.AspNetCore.Mvc;

namespace HexaPokerNet.WebApi.Controllers;

/// <summary>
/// Health API.
/// </summary>
[ApiController]
[Route("health")]
public class HealthController : Controller
{
    /// <summary>
    /// Returns a service health status.
    /// </summary>
    /// <returns>HTTP 200 when service is ready to handle requests</returns>
    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}