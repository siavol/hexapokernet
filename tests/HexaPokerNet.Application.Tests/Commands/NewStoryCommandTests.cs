namespace HexaPokerNet.Application.Tests;

using Commands;
using Mocks;

[TestFixture]
public class NewStoryCommandTests
{
    private InMemoryRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new InMemoryRepository();
    }

    [Test]
    public async Task CreateCommand()
    {
        var command = new NewStoryCommand("My test command", _repository);
        var story = await command.Execute();
        Assert.That(story.Title, Is.EqualTo("My test command"));
    }
}
