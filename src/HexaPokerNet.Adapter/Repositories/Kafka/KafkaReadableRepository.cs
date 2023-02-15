using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Application.Infrastructure;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaReadableRepository : IReadableRepository, IDisposable
{
    private const int WaitStoryEventToBeHandled = 2;
    private const int HealthySilenceTimeoutInSeconds = 5;

    private readonly ILogger<KafkaReadableRepository> _logger;
    private readonly Dictionary<string, Story> _stories = new();
    private readonly KafkaEntityEventConsumer _consumer;

    public KafkaReadableRepository(
        IKafkaConfiguration configuration,
        AggregatedHealthProvider aggregatedHealthProvider,
        ILogger<KafkaReadableRepository> logger)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var consumerId = Guid.NewGuid();
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration.KafkaServer,
            GroupId = $"hexapokernet-{consumerId}",
            ClientId = $"hexapokernet-read-repo-{consumerId}",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var kafkaConsumer = new ConsumerBuilder<string, IEntityEvent>(consumerConfig)
            .SetValueDeserializer(new EntityEventKafkaDeserializer())
            .Build();

        var healthTrackerStrategy = CreateConsumerHealthTrackerStrategy(logger);
        _consumer = new KafkaEntityEventConsumer(kafkaConsumer, healthTrackerStrategy);
        aggregatedHealthProvider.RegisterHealthProvider(healthTrackerStrategy.HealthTracker);

        _logger.LogDebug("Kafka readable repository created for {KafkaServer}", configuration.KafkaServer);
    }

    private static ConsumerErrorHealthTrackerStrategy CreateConsumerHealthTrackerStrategy(ILogger<KafkaReadableRepository> logger)
    {
        var errorStrategy = new ConsumerErrorWaitStrategy(logger);
        var healthTracker = new HealthLoggingTracker(logger);
        var healthTrackerStrategy = new ConsumerErrorHealthTrackerStrategy(
            healthTracker, errorStrategy, TimeSpan.FromSeconds(HealthySilenceTimeoutInSeconds));
        return healthTrackerStrategy;
    }

    public Task<Story> GetStoryById(string storyId)
    {
        SpinWait.SpinUntil(
            () => _stories.ContainsKey(storyId),
            TimeSpan.FromSeconds(WaitStoryEventToBeHandled));

        if (_stories.TryGetValue(storyId, out var story))
            return Task.FromResult(story);
        throw new EntityNotFoundException();
    }

    public void Start()
    {
        Task.Run(() =>
        {
            _logger.LogInformation("Start kafka event consuming task");
            _consumer.SubscribeTopics();
            try
            {
                while (true)
                {
                    var entityEvent = _consumer.ConsumeNextEvent();
                    HandleEvent(entityEvent);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Kafka consumer task is stopped");
            }
        });
    }

    private void HandleEvent(IEntityEvent? entityEvent)
    {
        switch (entityEvent)
        {
            case StoryAddedEvent storyAdded:
                _logger.LogInformation("Consumed a Story Added event from Kafka: {StoryId}", storyAdded.StoryId);
                _stories.Add(storyAdded.StoryId, storyAdded.GetEntity());
                break;
        }
    }

    void IDisposable.Dispose()
    {
        _consumer.Dispose();
    }
}