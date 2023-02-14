using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public interface IKafkaEntityEventConsumerErrorStrategy
{
    ConsumeResult<string, IEntityEvent>? GetErrorResult(ConsumeException e);
}

public class ConsumerErrorWaitStrategy : IKafkaEntityEventConsumerErrorStrategy
{
    private readonly ILogger _logger;
    private const int TimeoutAfterConsumeErrorInSeconds = 1;

    public ConsumerErrorWaitStrategy(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ConsumeResult<string, IEntityEvent>? GetErrorResult(ConsumeException e)
    {
        _logger.LogWarning("Failed to consume messages - {Message}. Wait {Timeout} second(s)",
            e.Message, TimeoutAfterConsumeErrorInSeconds);
        Thread.Sleep(TimeSpan.FromSeconds(TimeoutAfterConsumeErrorInSeconds));
        return null;
    }
}
