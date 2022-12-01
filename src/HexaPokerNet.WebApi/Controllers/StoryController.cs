using System.ComponentModel.DataAnnotations;
using HexaPokerNet.Application.Commands;
using HexaPokerNet.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HexaPokerNet.WebApi.Controllers;

/// <summary>
/// Stories API.
/// </summary>
[ApiController]
[Route("story")]
public class StoryController : Controller
{
    /// <summary>
    /// Creates a new Story.
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="writableRepository"></param>
    /// <returns>A newly created Story.</returns>
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] AddStoryParameters parameters,
        [FromServices] IWritableRepository writableRepository)
    {
        var command = new NewStoryCommand(parameters.Title, writableRepository);
        var story = await command.Execute();
        return Ok(story);
    }
}

/// <summary>
/// Add new Story parameters.
/// </summary>
public class AddStoryParameters
{
    /// <summary>
    /// New Story title.
    /// </summary>
    [Required]
    public string Title { get; set; } = null!;
}