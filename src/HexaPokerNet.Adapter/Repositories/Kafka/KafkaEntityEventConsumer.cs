using Confluent.Kafka;
using HexaPokerNet.Application.Events;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaEntityEventConsumer : IDisposable
{
    private readonly IConsumer<string, IEntityEvent> _consumer;
    private readonly IKafkaEntityEventConsumerErrorStrategy _errorStrategy;
    private readonly CancellationTokenSource _consumerTaskCancellationTokenSource = new();

    public KafkaEntityEventConsumer(IConsumer<string, IEntityEvent> consumer, IKafkaEntityEventConsumerErrorStrategy errorStrategy)
    {
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _errorStrategy = errorStrategy ?? throw new ArgumentNullException(nameof(errorStrategy));
    }

    public void SubscribeTopics()
    {
        _consumer.Subscribe(KafkaTopic.EntityEvents);
    }

    public IEntityEvent? ConsumeNextEvent()
    {
        var result = ConsumeAndWaitIfError();
        return result?.Message?.Value;
    }

    private ConsumeResult<string, IEntityEvent>? ConsumeAndWaitIfError()
    {
        try
        {
            return _consumer.Consume(_consumerTaskCancellationTokenSource.Token);
        }
        catch (ConsumeException e)
        {
            return _errorStrategy.GetErrorResult(e);
        }
    }

    public void Dispose()
    {
        _consumer.Dispose();
        _consumerTaskCancellationTokenSource.Cancel();
        _consumerTaskCancellationTokenSource.Dispose();
    }
}
