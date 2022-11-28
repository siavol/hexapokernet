namespace HexaPokerNet.Application.Repositories;

using HexaPokerNet.Domain;

public interface IWritableRepository
{
    Task<Story> CreateStory(string title);
}