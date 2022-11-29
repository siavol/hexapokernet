namespace HexaPokerNet.Application.Repositories;

using HexaPokerNet.Domain;

public interface IWritableRepository
{
    Task AddStory(Story story);
}