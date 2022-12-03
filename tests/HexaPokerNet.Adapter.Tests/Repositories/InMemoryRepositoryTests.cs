using HexaPokerNet.Adapter.Repositories;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;

namespace HexaPokerNet.Adapter.Tests.Repositories;

[TestFixture]
public class InMemoryRepositoryTests
{
    private IWritableRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        _repository = new InMemoryRepository();
    }

    [Test]
    public void AddStory()
    {
        _repository.AddStory(new Story("story1", "My test story"));
        Assert.Pass();
    }
}