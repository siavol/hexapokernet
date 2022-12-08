using System.ComponentModel.Design;

namespace HexaPokerNet.Application.Events;

public interface IEntityEvent
{
    
}

public interface IEntityEventHandler :
    IEntityEventHandler<StoryAddedEvent>
{
    
}

public interface IEntityEventHandler<in T> where T : IEntityEvent
{
    Task HandleEvent(T entityEvent);
}