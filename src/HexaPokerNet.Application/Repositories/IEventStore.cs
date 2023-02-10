using HexaPokerNet.Application.Events;

namespace HexaPokerNet.Application.Repositories;

public interface IEventStore
{
    Task RegisterEvent(IEntityEvent entityEvent);
}