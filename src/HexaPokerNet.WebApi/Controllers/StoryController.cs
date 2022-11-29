using System.ComponentModel.DataAnnotations;
using HexaPokerNet.Application.Commands;
using HexaPokerNet.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HexaPokerNet.WebApi.Controllers;

[ApiController]
[Route("story")]
public class StoryController : Controller
{
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

public  class AddStoryParameters
{
    [Required]
    public string Title { get; set; }
}