using NUnit.Framework;

namespace HexaPokerNet.Adapter.Tests;

[TestFixture]
public class EntityIdGeneratorTests
{
    private EntityIdGenerator _idGenerator = null!;

    [SetUp]
    public void Setup()
    {
        _idGenerator = new EntityIdGenerator();
    }

    [Test]
    public void ReturnsNotEmptyString()
    {
        var newId = _idGenerator.NewId();
        Assert.That(newId, Is.Not.Empty);
    }

    [Test]
    public void GeneratesUniqueStrings()
    {
        var newId1 = _idGenerator.NewId();
        var newId2 = _idGenerator.NewId();
        Assert.That(newId1, Is.Not.EqualTo(newId2));
    }
}