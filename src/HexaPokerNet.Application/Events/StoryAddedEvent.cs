using System.Text.Json.Serialization;
using HexaPokerNet.Domain;

namespace HexaPokerNet.Application.Events;

public class StoryAddedEvent : IEntityEvent
{
    [JsonPropertyName("id")]
    public string StoryId { get; }

    public string StoryTitle { get; }

    string IEntityEvent.EntityKey => StoryId;

    public StoryAddedEvent(string storyId, string storyTitle)
    {
        StoryId = storyId;
        StoryTitle = storyTitle;
    }

    public Story GetEntity()
    {
        return new Story(StoryId, StoryTitle);
    }
}