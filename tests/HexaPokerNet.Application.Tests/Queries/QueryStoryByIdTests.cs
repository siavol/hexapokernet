using HexaPokerNet.Application.Queries;
using HexaPokerNet.Application.Tests.Mocks;
using HexaPokerNet.Domain;

namespace HexaPokerNet.Application.Tests.Queries;

[TestFixture]
public class QueryStoryByIdTests
{
    private InMemoryRepository _repository = null!;

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

        var query = new QueryStoryById(storyId, _repository);
        var story = await query.Query();
        Assert.That(story, Is.SameAs(existingStory));
    }
}