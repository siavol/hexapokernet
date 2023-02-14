using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaEntityEventConsumer : IDisposable
{
    private const int TimeoutAfterConsumeErrorInSeconds = 1;
    private readonly IConsumer<string, IEntityEvent> _consumer;
    private readonly ILogger _logger;
    private readonly CancellationTokenSource _consumerTaskCancellationTokenSource = new();

    public KafkaEntityEventConsumer(IConsumer<string, IEntityEvent> consumer, ILogger logger)
    {
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            _logger.LogWarning("Failed to consume messages - {Message}. Wait {Timeout} second(s)",
                e.Message, TimeoutAfterConsumeErrorInSeconds);
            Thread.Sleep(TimeSpan.FromSeconds(TimeoutAfterConsumeErrorInSeconds));
            return null;
        }
    }

    public void Dispose()
    {
        _consumer.Dispose();
        _consumerTaskCancellationTokenSource.Cancel();
        _consumerTaskCancellationTokenSource.Dispose();
    }
}