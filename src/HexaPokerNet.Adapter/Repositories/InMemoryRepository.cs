namespace HexaPokerNet.Adapter.Repositories;

using System;
using HexaPokerNet.Domain;
using HexaPokerNet.Application.Repositories;

public class InMemoryRepository: IWritableRepository
{
    private readonly List<Story> _stories = new();

    Task IWritableRepository.AddStory(Story story)
    {
        if (story is null)
        {
            throw new ArgumentNullException(nameof(story));
        }

        return Task.Run(() => {
            this._stories.Add(story);
        });
    }
}
