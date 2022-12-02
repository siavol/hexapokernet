using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;

namespace HexaPokerNet.Application.Queries;

public class GetStoryByIdQuery
{
    private readonly string _storyId;
    private readonly IReadableRepository _repository;

    public GetStoryByIdQuery(string storyId, IReadableRepository repository)
    {
        _storyId = storyId ?? throw new ArgumentNullException(nameof(storyId));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Story> Query()
    {
        return await _repository.GetStoryById(_storyId);
    }
}