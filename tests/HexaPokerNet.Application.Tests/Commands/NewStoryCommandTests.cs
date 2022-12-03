using HexaPokerNet.Application.Commands;
using HexaPokerNet.Application.Tests.Mocks;
using HexaPokerNet.Domain;
using Moq;

namespace HexaPokerNet.Application.Tests.Commands;

[TestFixture]
public class NewStoryCommandTests
{
    private InMemoryRepository _repository = null!;
    private Mock<IEntityIdGenerator> _idGeneratorMock = null!;

    [SetUp]
    public void Setup()
    {
        _repository = new InMemoryRepository();
        _idGeneratorMock = new Mock<IEntityIdGenerator>();
    }

    [Test]
    public async Task CreateCommand()
    {
        _idGeneratorMock.Setup(idGen => idGen.NewId()).Returns("story1");
        
        var command = new NewStoryCommand("My test command", _repository, _idGeneratorMock.Object);
        var story = await command.Execute();
        Assert.That(story.Title, Is.EqualTo("My test command"));
        Assert.That(story.Id, Is.EqualTo("story1"));
    }
}
