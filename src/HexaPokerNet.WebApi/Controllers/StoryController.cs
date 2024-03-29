﻿using System.ComponentModel.DataAnnotations;
using HexaPokerNet.Application.Commands;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
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
    /// <param name="eventStore"></param>
    /// <param name="idGenerator"></param>
    /// <returns>A newly created Story.</returns>
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] AddStoryParameters parameters,
        [FromServices] IEventStore eventStore,
        [FromServices] IEntityIdGenerator idGenerator)
    {
        var command = new NewStoryCommand(parameters.Title, eventStore, idGenerator);
        var storyId = await command.Execute();
        return Ok(new
        {
            id = storyId
        });
    }

    /// <summary>
    /// Returns a story by id.
    /// </summary>
    /// <param name="storyId"></param>
    /// <param name="readableRepository"></param>
    /// <returns></returns>
    [HttpGet("{storyId}")]
    public async Task<IActionResult> GetStory(
        string storyId,
        IReadableRepository readableRepository)
    {
        try
        {
            var story = await readableRepository.GetStoryById(storyId);
            return new OkObjectResult(story);
        }
        catch (EntityNotFoundException)
        {
            return new NotFoundResult();
        }
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