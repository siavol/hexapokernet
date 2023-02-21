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
    [CosmosDBOutput(
        databaseName: StorageConstants.InputEventsDatabaseName,
        containerName: StorageConstants.EntityEventsContainerName,
        Connection = StorageConstants.CosmosConnectionSettingName,
        CreateIfNotExists = true)]
    public async Task<StoryAddedEvent> PostStory(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        var input = await req.ReadFromJsonAsync<AddStoryParameters>();
        var storyId = _entityIdGenerator.NewId();
        var storyAddedEvent = new StoryAddedEvent(storyId, input.Title);
        return storyAddedEvent;
    }

    [Function("GetStoryById")]
    public async Task<HttpResponseData> GetStory(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Story/{storyId}")]
        HttpRequestData req,
        [CosmosDBInput(
            databaseName: StorageConstants.MaterializedEntitiesDatabaseName,
            containerName: StorageConstants.MaterializedStoriesContainerName,
            Connection = StorageConstants.CosmosConnectionSettingName,
            SqlQuery = "SELECT * FROM c WHERE c.id = {storyId}")
        ]
        IEnumerable<StoryEntity> queryResults)
    {
        var res = req.CreateResponse();

        var story = queryResults.SingleOrDefault();
        if (story == null)
        {
            _logger.LogInformation("Story not found.");
            res.StatusCode = HttpStatusCode.NotFound;
        }
        else
        {
            res.StatusCode = HttpStatusCode.OK;
            await res.WriteAsJsonAsync(story);
        }

        return res;
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
