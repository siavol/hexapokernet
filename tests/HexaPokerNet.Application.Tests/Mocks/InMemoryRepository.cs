namespace HexaPokerNet.Application.Tests.Mocks;

using HexaPokerNet.Domain;
using HexaPokerNet.Application.Repositories;

class InMemoryRepository : IWritableRepository
{
    Task IWritableRepository.AddStory(Story story)
    {
        return Task.Run(() => {});
    }
}