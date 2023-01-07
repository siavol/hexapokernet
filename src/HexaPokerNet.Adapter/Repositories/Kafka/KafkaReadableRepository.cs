using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaReadableRepository: IReadableRepository, IDisposable
{
    private readonly Dictionary<string, Story> _stories = new();
    private readonly IConsumer<string, string> _consumer;
    private readonly CancellationTokenSource _consumerTaskCancellationTokenSource = new();
    private Task? _consumerTask;

    public KafkaReadableRepository(string kafkaServer)
    {
        var consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = kafkaServer,
            GroupId = "hexapokernet",
            EnableAutoCommit = true,
            EnableAutoOffsetStore = true,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig)
            .SetErrorHandler((_, err) => Console.WriteLine(err))
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
            while (!_consumerTaskCancellationTokenSource.IsCancellationRequested)
            {
                var result = _consumer.Consume();
                var message = result?.Message;
                if (message != null)
                {
                    var entityEvent = EntityEventSerializer.Deserialize<IEntityEvent>(message.Value);
                    if (entityEvent is StoryAddedEvent storyAdded)
                    {
                        _stories.Add(storyAdded.StoryId, storyAdded.GetEntity());
                    }
                }
            }
        });
    }
    
    public void Dispose()
    {
        _consumer.Dispose();
        _consumerTaskCancellationTokenSource.Dispose();
        _consumerTask?.Dispose();
    }
}