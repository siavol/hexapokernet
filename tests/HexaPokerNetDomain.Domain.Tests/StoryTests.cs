namespace HexaPokerNetDomain.Domain.Tests;

using HexaPokerNet.Domain;

[TestFixture]
public class StoryTests
{
    [Test]
    public void CreateNewStory()
    {
        var story = new Story("My test story");
        Assert.That(story.Title, Is.EqualTo("My test story"));
    }

    [Test]
    public void CreateNewStoryThrowsWhenTitleIsNull()
    {
#nullable disable
        Assert.That(
            () => new Story(null),
            Throws.InstanceOf<ArgumentNullException>());
#nullable restore
    }
}