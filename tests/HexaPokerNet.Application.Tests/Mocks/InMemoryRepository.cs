namespace HexaPokerNet.Application.Tests.Mocks;

using Domain;
using Repositories;

class InMemoryRepository : IReadableRepository
{
    private readonly Dictionary<string, Story> _storiesStorage = new();

    public Task AddStory(Story story)
    {
        if (story == null) throw new ArgumentNullException(nameof(story));
        _storiesStorage[story.Id] = story;
        return Task.CompletedTask;
    }

    public void Start()
    {
    }

    public Task<Story> GetStoryById(string storyId)
    {
        if (!_storiesStorage.ContainsKey(storyId))
        {
            throw new EntityNotFoundException();
        }

        return Task.FromResult(_storiesStorage[storyId]);
    }
}