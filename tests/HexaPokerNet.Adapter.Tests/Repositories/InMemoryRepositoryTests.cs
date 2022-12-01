namespace HexaPokerNet.Adapter.Tests;

using HexaPokerNet.Domain;
using HexaPokerNet.Adapter.Repositories;
using HexaPokerNet.Application.Repositories;

[TestFixture]
public class InMemoryRepositoryTests
{
    private IWritableRepository _repository;

    [SetUp]
    public void Setup()
    {
        this._repository = new InMemoryRepository();
    }

    [Test]
    public void AddStory()
    {
        this._repository.AddStory(new Story("My test story"));
        Assert.Pass();
    }
}