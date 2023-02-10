namespace HexaPokerNet.Application.Events;

public interface IEntityEvent
{
    string EntityKey { get; }
}

public interface IEntityEventHandler :
    IEntityEventHandler<StoryAddedEvent>
{

}

public interface IEntityEventHandler<in T> where T : IEntityEvent
{
    Task HandleEvent(T entityEvent);
}