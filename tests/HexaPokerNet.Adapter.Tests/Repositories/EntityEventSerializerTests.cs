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
}