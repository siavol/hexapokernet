using HexaPokerNet.Domain;

namespace HexaPokerNet.Application.Repositories;

public interface IReadableRepository
{
    Task<Story> GetStoryById(string storyId);
}