using Microsoft.AspNetCore.Mvc;

namespace HexaPokerNet.WebApi.Controllers;

/// <summary>
/// Health API.
/// </summary>
[ApiController]
[Route("health")]
public class HealthController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok();
    }
}