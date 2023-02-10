using HexaPokerNet.Application.Events;

namespace HexaPokerNet.Adapter.Repositories;

using System;
using Domain;
using HexaPokerNet.Application.Repositories;

public class InMemoryRepository : IEventStore, IEntityEventHandler, IReadableRepository
{
    private readonly Dictionary<string, Story> _stories = new();

    public async Task RegisterEvent(IEntityEvent entityEvent)
    {
        if (entityEvent == null) throw new ArgumentNullException(nameof(entityEvent));
        await this.HandleEvent((dynamic)entityEvent);
    }

    public void Start()
    {
    }

    public Task<Story> GetStoryById(string storyId)
    {
        if (!_stories.TryGetValue(storyId, out var story))
        {
            throw new EntityNotFoundException();
        }

        return Task.FromResult(story);
    }

    public Task HandleEvent(StoryAddedEvent entityEvent)
    {
        if (entityEvent == null) throw new ArgumentNullException(nameof(entityEvent));

        var story = new Story(entityEvent.StoryId, entityEvent.StoryTitle);
        _stories.Add(story.Id, story);
        return Task.CompletedTask;
    }
}
