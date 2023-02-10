using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaReadableRepository : IReadableRepository, IDisposable
{
    private readonly ILogger<KafkaReadableRepository> _logger;
    private readonly Dictionary<string, Story> _stories = new();
    private readonly IConsumer<string, IEntityEvent> _consumer;
    private readonly CancellationTokenSource _consumerTaskCancellationTokenSource = new();

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

        _consumer = new ConsumerBuilder<string, IEntityEvent>(consumerConfig)
            .SetErrorHandler((_, err) => Console.WriteLine(err))
            .SetValueDeserializer(new EntityEventKafkaDeserializer())
            .Build();
    }

    public Task<Story> GetStoryById(string storyId)
    {
        if (!_stories.TryGetValue(storyId, out var story))
        {
            throw new EntityNotFoundException();
        }

        return Task.FromResult(story);
    }

    public void Start()
    {
        Task.Run(() =>
        {
            _consumer.Subscribe("entityEvents");
            try
            {
                while (true)
                {
                    var result = _consumer.Consume(_consumerTaskCancellationTokenSource.Token);
                    var entityEvent = result?.Message?.Value;
                    if (entityEvent is StoryAddedEvent storyAdded)
                    {
                        _logger.LogInformation("Consumed a Story Added event from Kafka: {0}", storyAdded.StoryId);
                        _stories.Add(storyAdded.StoryId, storyAdded.GetEntity());
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Kafka consumer task is stopped.");
            }
        });
    }

    public void Dispose()
    {
        _consumer.Dispose();
        _consumerTaskCancellationTokenSource.Cancel();
        _consumerTaskCancellationTokenSource.Dispose();
    }
}