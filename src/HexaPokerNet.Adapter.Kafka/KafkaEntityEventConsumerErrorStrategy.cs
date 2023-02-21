using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Application.Infrastructure;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Adapter.Kafka;

public interface IKafkaEntityEventConsumerErrorStrategy
{
    ConsumeResult<string, IEntityEvent>? GetErrorResult(ConsumeException e);
    void ConsumedSuccessfully();
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

    void IKafkaEntityEventConsumerErrorStrategy.ConsumedSuccessfully()
    {
    }
}

public class ConsumerErrorHealthTrackerStrategy : IKafkaEntityEventConsumerErrorStrategy
{
    private readonly IKafkaEntityEventConsumerErrorStrategy _innerStrategy;
    private readonly TimeSpan _healthySilenceTimeout;
    private Timer? _silenceTimer;
    private AutoResetEvent? _silenceAutoEvent;

    public HealthTracker HealthTracker { get; }

    public ConsumerErrorHealthTrackerStrategy(
        HealthTracker healthTracker,
        IKafkaEntityEventConsumerErrorStrategy innerStrategy,
        TimeSpan healthySilenceTimeout)
    {
        HealthTracker = healthTracker ?? throw new ArgumentNullException(nameof(healthTracker));
        _innerStrategy = innerStrategy ?? throw new ArgumentNullException(nameof(innerStrategy));
        _healthySilenceTimeout = healthySilenceTimeout;

        StartSilenceTimer();
    }

    private void StartSilenceTimer()
    {
        StopSilenceTimer();
        _silenceAutoEvent = new AutoResetEvent(false);
        _silenceTimer = new Timer(SilenceTimerCallback, _silenceAutoEvent, _healthySilenceTimeout, Timeout.InfiniteTimeSpan);
    }

    private void SilenceTimerCallback(object? state)
    {
        StopSilenceTimer();
        _silenceTimer = null;
        HealthTracker.ReportHealthStatus(HealthStatus.Healthy);
    }

    private void StopSilenceTimer()
    {
        _silenceAutoEvent?.Set();
        _silenceTimer?.Dispose();
    }

    public ConsumeResult<string, IEntityEvent>? GetErrorResult(ConsumeException e)
    {
        RestartSilenceTimerWhenAppIsStarting();
        return _innerStrategy.GetErrorResult(e);
    }

    public void ConsumedSuccessfully()
    {
        RestartSilenceTimerWhenAppIsStarting();
        _innerStrategy.ConsumedSuccessfully();
    }

    private void RestartSilenceTimerWhenAppIsStarting()
    {
        if (HealthTracker.HealthStatus == HealthStatus.Starting)
        {
            StartSilenceTimer();
        }
    }
}
