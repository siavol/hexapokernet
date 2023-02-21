namespace HexaPokerNet.Azure;

internal static class StorageConstants
{
    public const string CosmosConnectionSettingName = "CosmosConnection";

    public const string InputEventsDatabaseName = "input-events";
    public const string EntityEventsContainerName = "entity-events";

    public const string MaterializedEntitiesDatabaseName = "materialized-entities";
    public const string MaterializedStoriesContainerName = "stories";
}