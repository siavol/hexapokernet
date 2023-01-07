using HexaPokerNet.Adapter.Repositories;
using HexaPokerNet.Application.Events;

namespace HexaPokerNet.Adapter.Tests.Repositories;

[TestFixture]
public class EntityEventSerializerTests
{
    [Test]
    public void Serialize_StoryAddedEvent()
    {
        var storyAddedEvent = new StoryAddedEvent("story1", "My test story");
        var result = EntityEventSerializer.Serialize(storyAddedEvent);
        Assert.That(result,
            Is.EqualTo("{\"$type\":\"StoryAddedEvent\",\"StoryId\":\"story1\",\"StoryTitle\":\"My test story\"}"));
    }

    [Test]
    public void Deserialize_ReturnsStory()
    {
        var json = "{\"$type\":\"StoryAddedEvent\",\"StoryId\":\"story1\",\"StoryTitle\":\"My test story\"}";
        var story = EntityEventSerializer.Deserialize<StoryAddedEvent>(json);

        Assert.True(story is
        {
            StoryId: "story1",
            StoryTitle: "My test story"
        });
    }

    [Test]
    public void Deserialize_ReturnsGeneralEntityEvent()
    {
        const string json = "{\"$type\":\"StoryAddedEvent\",\"StoryId\":\"story1\",\"StoryTitle\":\"My test story\"}";
        var entityEvent = EntityEventSerializer.Deserialize<IEntityEvent>(json);

        Assert.IsInstanceOf<StoryAddedEvent>(entityEvent);
        var storyAddedEvent = (StoryAddedEvent)entityEvent;
        Assert.True(storyAddedEvent is
        {
            StoryId: "story1",
            StoryTitle: "My test story"
        });
    }
}