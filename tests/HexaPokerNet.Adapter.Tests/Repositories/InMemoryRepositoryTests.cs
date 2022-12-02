namespace HexaPokerNet.Adapter.Tests;

using Domain;
using Repositories;
using HexaPokerNet.Application.Repositories;

[TestFixture]
public class InMemoryRepositoryTests
{
    private IWritableRepository _repository;

    [SetUp]
    public void Setup()
    {
        _repository = new InMemoryRepository();
    }

    [Test]
    public void AddStory()
    {
        _repository.AddStory(new Story("My test story"));
        Assert.Pass();
    }
}