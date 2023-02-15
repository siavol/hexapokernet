using System.Net;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Domain;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Azure;

public class StoryFunction
{
    private readonly IEntityIdGenerator _entityIdGenerator;
    private readonly ILogger<StoryFunction> _logger;

    public StoryFunction(IEntityIdGenerator entityIdGenerator, ILogger<StoryFunction> logger)
    {
        _entityIdGenerator = entityIdGenerator ?? throw new ArgumentNullException(nameof(entityIdGenerator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [Function("Story")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        var input = await req.ReadFromJsonAsync<AddStoryParameters>();
        var storyId = _entityIdGenerator.NewId();
        var storyAddedEvent = new StoryAddedEvent(input.Title, storyId);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(storyAddedEvent);
        return response;
    }
}

public class AddStoryParameters
{
    /// <summary>
    /// New Story title.
    /// </summary>
    // [Required]
    public string Title { get; set; } = null!;
}
