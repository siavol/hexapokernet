namespace HexaPokerNet.Adapter.Repositories;

using System;
using Domain;
using HexaPokerNet.Application.Repositories;

public class InMemoryRepository : IWritableRepository, IReadableRepository
{
    private readonly Dictionary<string, Story> _stories = new();

    Task IWritableRepository.AddStory(Story story)
    {
        if (story is null)
        {
            throw new ArgumentNullException(nameof(story));
        }

        return Task.Run(() =>
        {
            _stories.Add(story.Id, story);
        });
    }

    Task<Story> IReadableRepository.GetStoryById(string storyId)
    {
        if (!_stories.TryGetValue(storyId, out var story))
        {
            throw new EntityNotFoundException();
        }

        return Task.FromResult(story);
    }
}
