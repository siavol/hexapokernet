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
    public IEnumerable<StoryEntity> OnEntityEvent([CosmosDBTrigger(
            databaseName: StorageConstants.InputEventsDatabaseName,
            containerName: StorageConstants.EntityEventsContainerName,
            Connection = StorageConstants.CosmosConnectionSettingName,
            LeaseDatabaseName = StorageConstants.MaterializedEntitiesDatabaseName,
            CreateLeaseContainerIfNotExists = true)]
        IReadOnlyCollection<StoryAddedEvent> events)
    {
        return events
            .Select(e => new StoryEntity
            {
                Id = e.StoryId,
                Title = e.StoryTitle
            });
    }
}