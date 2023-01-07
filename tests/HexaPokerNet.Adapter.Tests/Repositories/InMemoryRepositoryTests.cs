using HexaPokerNet.Adapter.Repositories;
using HexaPokerNet.Application.Events;

namespace HexaPokerNet.Adapter.Tests.Repositories;

[TestFixture]
public class InMemoryRepositoryTests
{
    private InMemoryRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        _repository = new InMemoryRepository();
    }

    [Test]
    public async Task StoryAddedEvent()
    {
        var storyAddedEvent = new StoryAddedEvent("story1", "My test story");
        await _repository.RegisterEvent(storyAddedEvent);

        var storyFromRepo = await _repository.GetStoryById("story1");
        Assert.Multiple(() =>
        {
            Assert.That(storyFromRepo.Id, Is.EqualTo("story1"));
            Assert.That(storyFromRepo.Title, Is.EqualTo("My test story"));
        });
    }
}