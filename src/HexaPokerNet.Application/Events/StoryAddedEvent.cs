namespace HexaPokerNet.Application.Events;

public class StoryAddedEvent : IEntityEvent
{
    public string StoryId { get; }
    public string StoryTitle { get; }

    string IEntityEvent.EntityKey => StoryId;

    public StoryAddedEvent(string storyId, string storyTitle)
    {
        StoryId = storyId;
        StoryTitle = storyTitle;
    }
}