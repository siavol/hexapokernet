namespace HexaPokerNet.Application.Commands;

using HexaPokerNet.Domain;
using HexaPokerNet.Application.Repositories;

public class NewStoryCommand
{
    private readonly IWritableRepository _repository;
    private readonly string _title;

    public NewStoryCommand(string title, IWritableRepository repository)
    {
        this._title = title ?? throw new ArgumentNullException(nameof(title));
        this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Story> Execute()
    {
        return await this._repository.CreateStory(this._title);
    }
}
