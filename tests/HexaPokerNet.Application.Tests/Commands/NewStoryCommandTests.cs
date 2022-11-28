namespace HexaPokerNet.Application.Tests;

using HexaPokerNet.Application.Commands;
using HexaPokerNet.Application.Tests.Mocks;

[TestFixture]
public class NewStoryCommandTests
{
    private InMemoryRepository _repository;

    [SetUp]
    public void Setup()
    {
        this._repository = new InMemoryRepository();
    }

    [Test]
    public async Task CreateCommand()
    {
        var command = new NewStoryCommand("My test command", this._repository);
        var story = await command.Execute();
        Assert.That(story.Title, Is.EqualTo("My test command"));
    }
}
