namespace HexaPokerNetDomain.Domain.Tests;

using HexaPokerNet.Domain;

[TestFixture]
public class StoryTests
{
    [Test]
    public void CreateNewStory()
    {
        var story = new Story("story1", "My test story");
        Assert.That(story.Title, Is.EqualTo("My test story"));
        Assert.That(story.Id, Is.EqualTo("story1"));
    }

    [Test]
    public void CreateStoryInstance()
    {
        var story = new Story("id123", "My existing story");
        Assert.That(story.Title, Is.EqualTo("My existing story"));
        Assert.That(story.Id, Is.EqualTo("id123"));
    }

    [Test]
    public void CreateNewStoryThrowsWhenTitleIsNull()
    {
#nullable disable
        Assert.That(
            () => new Story("story1", null),
            Throws.InstanceOf<ArgumentNullException>());
#nullable restore
    }
}