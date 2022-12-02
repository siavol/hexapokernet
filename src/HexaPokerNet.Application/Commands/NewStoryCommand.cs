namespace HexaPokerNet.Application.Commands;

using Domain;
using Repositories;

public class NewStoryCommand
{
    private readonly IWritableRepository _repository;
    private readonly string _title;

    public NewStoryCommand(string title, IWritableRepository repository)
    {
        _title = title ?? throw new ArgumentNullException(nameof(title));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Story> Execute()
    {
        var story = new Story(_title);
        await _repository.AddStory(story);
        return story;
    }
}
