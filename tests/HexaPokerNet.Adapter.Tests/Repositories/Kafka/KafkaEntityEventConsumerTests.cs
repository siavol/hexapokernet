using Confluent.Kafka;
using HexaPokerNet.Adapter.Repositories.Kafka;
using HexaPokerNet.Application.Events;
using Moq;

namespace HexaPokerNet.Adapter.Tests.Repositories.Kafka;

[TestFixture]
public class KafkaEntityEventConsumerTests
{
    private Mock<IConsumer<string, IEntityEvent>> _kafkaConsumerMock;
    private KafkaEntityEventConsumer _consumer;
    private Mock<IKafkaEntityEventConsumerErrorStrategy> _errorStrategyMock;

    [SetUp]
    public void Setup()
    {
        _kafkaConsumerMock = new Mock<IConsumer<string, IEntityEvent>>();
        _errorStrategyMock = new Mock<IKafkaEntityEventConsumerErrorStrategy>();
        _consumer = new KafkaEntityEventConsumer(_kafkaConsumerMock.Object, _errorStrategyMock.Object);
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
        var consumeResult = GetConsumeResult(newEvent);
        _kafkaConsumerMock.Setup(
            _ => _.Consume(It.IsAny<CancellationToken>())
        ).Returns(consumeResult);

        var entityEvent = _consumer.ConsumeNextEvent();
        Assert.That(entityEvent, Is.SameAs(newEvent));
    }

    
    [Test]
    public void ConsumeNextEventShouldNotifyErrorStrategyWhenConsumedSuccessfully()
    {
        var consumeResult = GetConsumeResult(new Mock<IEntityEvent>().Object);
        _kafkaConsumerMock.Setup(
            _ => _.Consume(It.IsAny<CancellationToken>())
        ).Returns(consumeResult);

        _consumer.ConsumeNextEvent();
        
        _errorStrategyMock.Verify(_ => _.ConsumedSuccessfully());
    }

    [Test]
    public void ConsumeNextEventShouldReturnErrorStrategyResultWhenConsumptionExceptionThrown()
    {
        var newEvent = new Mock<IEntityEvent>().Object;
        _kafkaConsumerMock.Setup(
            _ => _.Consume(It.IsAny<CancellationToken>())
        ).Throws(CreateConsumeException());
        _errorStrategyMock.Setup(
            _ => _.GetErrorResult(It.IsAny<ConsumeException>())
        ).Returns(GetConsumeResult(newEvent));

        var entityEvent = _consumer.ConsumeNextEvent();
        Assert.That(entityEvent, Is.SameAs(newEvent));
    }

    private static ConsumeException CreateConsumeException()
    {
        return new ConsumeException(new ConsumeResult<byte[], byte[]>(), new Error(ErrorCode.Unknown));
    }

    private static ConsumeResult<string, IEntityEvent> GetConsumeResult(IEntityEvent entityEvent)
    {
        return new ConsumeResult<string, IEntityEvent>
        {
            Message = new Message<string, IEntityEvent>
            {
                Value = entityEvent
            }
        };
    }
}