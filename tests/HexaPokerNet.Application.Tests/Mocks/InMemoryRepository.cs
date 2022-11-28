namespace HexaPokerNet.Application.Tests.Mocks;

using HexaPokerNet.Domain;
using HexaPokerNet.Application.Repositories;

class InMemoryRepository : IWritableRepository
{
    Task<Story> IWritableRepository.CreateStory(string title)
    {
        return Task.Run(() =>
        {
            var story = new Story(title);
            return story;
        });
    }
}