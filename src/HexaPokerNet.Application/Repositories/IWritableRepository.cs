namespace HexaPokerNet.Application.Repositories;

using Domain;

public interface IWritableRepository
{
    Task AddStory(Story story);
}