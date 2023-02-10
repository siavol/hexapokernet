using HexaPokerNet.Application.Commands;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Application.Tests.Mocks;
using HexaPokerNet.Domain;
using Moq;

namespace HexaPokerNet.Application.Tests.Commands;

[TestFixture]
public class NewStoryCommandTests
{
    private Mock<IEntityIdGenerator> _idGeneratorMock = null!;
    private Mock<IEventStore>? _eventStore = null;

    [SetUp]
    public void Setup()
    {
        _idGeneratorMock = new Mock<IEntityIdGenerator>();
        _eventStore = new Mock<IEventStore>();
    }

    [Test]
    public async Task CreateCommand()
    {
        _idGeneratorMock.Setup(idGen => idGen.NewId()).Returns("story1");

        var command = new NewStoryCommand("My test command", _eventStore!.Object, _idGeneratorMock.Object);
        var storyId = await command.Execute();
        Assert.That(storyId, Is.EqualTo("story1"));
    }
}
