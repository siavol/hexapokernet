using Confluent.Kafka;
using HexaPokerNet.Adapter.Repositories.Kafka;
using HexaPokerNet.Application.Events;
using Microsoft.Extensions.Logging;
using Moq;

namespace HexaPokerNet.Adapter.Tests.Repositories.Kafka;

[TestFixture]
public class KafkaEntityEventConsumerTests
{
    private Mock<ILogger> _loggerMock;
    private Mock<IConsumer<string, IEntityEvent>> _kafkaConsumerMock;
    private KafkaEntityEventConsumer _consumer;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger>();
        _kafkaConsumerMock = new Mock<IConsumer<string, IEntityEvent>>();
        _consumer = new KafkaEntityEventConsumer(_kafkaConsumerMock.Object, _loggerMock.Object);
    }

    [Test]
    public void SubscribeTopicsShouldSubscribeToEntityEventTopic()
    {
        _consumer.SubscribeTopics();
        _kafkaConsumerMock.Verify(kafkaConsumer => kafkaConsumer.Subscribe(KafkaTopic.EntityEvents));
    }

    [Test]
    public void ConsumeNextEventShouldReturnEventWhenConsumedSuccessfully()
    {
        var newEvent = new Mock<IEntityEvent>().Object;
        _kafkaConsumerMock.Setup(
            kafkaConsumer => kafkaConsumer.Consume(It.IsAny<CancellationToken>())
        ).Returns(new ConsumeResult<string, IEntityEvent>()
        {
            Message = new Message<string, IEntityEvent>()
            {
                Value = newEvent
            }
        });
        var entityEvent = _consumer.ConsumeNextEvent();

        Assert.That(entityEvent, Is.SameAs(newEvent));
    }
}