namespace HexaPokerNet.Application.Commands;

using Domain;
using Repositories;

public class NewStoryCommand
{
    private readonly IWritableRepository _repository;
    private readonly IEntityIdGenerator _idGenerator;
    private readonly string _title;

    public NewStoryCommand(string title, IWritableRepository repository, IEntityIdGenerator idGenerator)
    {
        _title = title ?? throw new ArgumentNullException(nameof(title));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    public async Task<Story> Execute()
    {
        var storyId = _idGenerator.NewId();
        var story = new Story(storyId, _title);
        await _repository.AddStory(story);
        return story;
    }
}
