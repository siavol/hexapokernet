using HexaPokerNet.Application.Events;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Azure;

public class EventHandlerFunction
{
    private readonly ILogger<EventHandlerFunction> _logger;

    public EventHandlerFunction(ILogger<EventHandlerFunction> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [Function("OnEntityEvent")]
    [CosmosDBOutput(
        databaseName: StorageConstants.MaterializedEntitiesDatabaseName, 
        containerName: StorageConstants.MaterializedStoriesContainerName,
        Connection = StorageConstants.CosmosConnectionSettingName,
        CreateIfNotExists = true)]
    public StoryEntity OnEntityEvent([CosmosDBTrigger(
            databaseName: StorageConstants.InputEventsDatabaseName,
            containerName: StorageConstants.EntityEventsContainerName,
            Connection = StorageConstants.CosmosConnectionSettingName)]
        IReadOnlyCollection<StoryAddedEvent> events)
    {
        return events
            .Select(e => new StoryEntity
            {
                Id = e.StoryId,
                Title = e.StoryTitle
            })
            .Single();
    }
}

public class StoryEntity
{
    public string Id { get; set; }
    public string Title { get; set; }
}