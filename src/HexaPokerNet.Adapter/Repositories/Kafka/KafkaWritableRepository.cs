using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
using Newtonsoft.Json;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaWritableRepository: IWritableRepository, IEventStore
{
    private const string _entityEventsTopic = "entityEvents";
    private readonly ProducerBuilder<string,string> _producerBuilder;

    public KafkaWritableRepository(string kafkaServer)
    {
        Dictionary<string, string> kafkaConfig = new()
        {
            { "bootstrap.servers", kafkaServer }
        };
        _producerBuilder = new ProducerBuilder<string, string>(kafkaConfig);
    }

    async Task IWritableRepository.AddStory(Story story)
    {
        if (story == null) throw new ArgumentNullException(nameof(story));

        var storyJson = JsonConvert.SerializeObject(story);
        using var producer = _producerBuilder.Build();
        await producer.ProduceAsync("entities",
            new Message<string, string> { Key = story.Id, Value = storyJson }
        );
    }

    public async Task RegisterEvent(IEntityEvent entityEvent)
    {
        if (entityEvent == null) throw new ArgumentNullException(nameof(entityEvent));

        var eventJson = EntityEventSerializer.Serialize(entityEvent);
        using var producer = _producerBuilder.Build();
        await producer.ProduceAsync(_entityEventsTopic,
            new Message<string, string> { Key = GetEventId(entityEvent), Value = eventJson }
        );
    }

    private string GetEventId(IEntityEvent entityEvent)
    {
        return new Guid().ToString("N");
    }
}