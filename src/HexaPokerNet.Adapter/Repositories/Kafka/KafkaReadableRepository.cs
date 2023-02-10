using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaReadableRepository : IReadableRepository, IDisposable
{
    private readonly Dictionary<string, Story> _stories = new();
    private readonly IConsumer<string, IEntityEvent> _consumer;
    private readonly CancellationTokenSource _consumerTaskCancellationTokenSource = new();
    private Task? _consumerTask;

    public KafkaReadableRepository(string kafkaServer)
    {
        var consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = kafkaServer,
            GroupId = $"hexapokernet{new Guid()}",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, IEntityEvent>(consumerConfig)
            .SetErrorHandler((_, err) => Console.WriteLine(err))
            .SetValueDeserializer(new EntityEventKafkaDeserializer())
            .Build();
        RunConsumerTask();
    }

    public Task<Story> GetStoryById(string storyId)
    {
        if (!_stories.TryGetValue(storyId, out var story))
        {
            throw new EntityNotFoundException();
        }

        return Task.FromResult(story);
    }

    private void RunConsumerTask()
    {
        _consumerTask = Task.Run(() =>
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
                        _stories.Add(storyAdded.StoryId, storyAdded.GetEntity());
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // TODO: add logging. For now we can just skip it.
            }
        });
    }

    public void Dispose()
    {
        _consumer.Dispose();
        _consumerTaskCancellationTokenSource.Cancel();
        _consumerTaskCancellationTokenSource.Dispose();
        _consumerTask?.Dispose();
    }
}