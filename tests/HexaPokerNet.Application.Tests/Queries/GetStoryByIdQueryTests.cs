using HexaPokerNet.Application.Commands;
using HexaPokerNet.Application.Queries;
using HexaPokerNet.Application.Tests.Mocks;
using HexaPokerNet.Domain;

namespace HexaPokerNet.Application.Tests.Queries;

[TestFixture]
public class GetStoryByIdQueryTests
{
    private InMemoryRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new InMemoryRepository();
    }

    [Test]
    public async Task ReturnsStoryForIdWhenItExistsInRepository()
    {
        const string storyId = "id1";
        var existingStory = new Story(storyId, "My test story");
        await _repository.AddStory(existingStory);

        var query = new GetStoryByIdQuery(storyId, _repository);
        var story = await query.Query();
        Assert.That(story, Is.SameAs(existingStory));
    }
}