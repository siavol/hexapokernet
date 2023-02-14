using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaReadableRepository : IReadableRepository, IDisposable
{
    private const int WaitStoryEventToBeHandled = 4;

    private readonly ILogger<KafkaReadableRepository> _logger;
    private readonly Dictionary<string, Story> _stories = new();
    private readonly KafkaEntityEventConsumer _consumer;

    public KafkaReadableRepository(IKafkaConfiguration configuration, ILogger<KafkaReadableRepository> logger)
    {
        _logger = logger;

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
        _consumer = new KafkaEntityEventConsumer(kafkaConsumer, logger);

        _logger.LogDebug("Kafka readable repository created for {KafkaServer}", configuration.KafkaServer);
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