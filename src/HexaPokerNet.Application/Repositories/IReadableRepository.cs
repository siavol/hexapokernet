using HexaPokerNet.Domain;

namespace HexaPokerNet.Application.Repositories;

public interface IReadableRepository
{
    void Start();
    Task<Story> GetStoryById(string storyId);
}