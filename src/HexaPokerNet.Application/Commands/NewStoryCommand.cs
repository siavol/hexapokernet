using HexaPokerNet.Application.Events;

namespace HexaPokerNet.Application.Commands;

using Domain;
using Repositories;

public class NewStoryCommand
{
    private readonly IEventStore _eventStore;
    private readonly IEntityIdGenerator _idGenerator;
    private readonly string _title;

    public NewStoryCommand(string title, IEventStore eventStore, IEntityIdGenerator idGenerator)
    {
        _title = title ?? throw new ArgumentNullException(nameof(title));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    public async Task<string> Execute()
    {
        var storyId = _idGenerator.NewId();
        var @event = new StoryAddedEvent(storyId, _title);
        await _eventStore.RegisterEvent(@event);
        return storyId;
    }
}
