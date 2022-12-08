using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Application.Repositories;
using HexaPokerNet.Domain;
using Newtonsoft.Json;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaEventStore: IEventStore
{
    private const string _entityEventsTopic = "entityEvents";
    private readonly ProducerBuilder<string,string> _producerBuilder;

    public KafkaEventStore(string kafkaServer)
    {
        Dictionary<string, string> kafkaConfig = new()
        {
            { "bootstrap.servers", kafkaServer }
        };
        _producerBuilder = new ProducerBuilder<string, string>(kafkaConfig);
    }
    
    public async Task RegisterEvent(IEntityEvent entityEvent)
    {
        if (entityEvent == null) throw new ArgumentNullException(nameof(entityEvent));

        var eventJson = EntityEventSerializer.Serialize(entityEvent);
        using var producer = _producerBuilder.Build();
        await producer.ProduceAsync(_entityEventsTopic,
            new Message<string, string> { Key = entityEvent.EntityKey, Value = eventJson }
        );
    }
}