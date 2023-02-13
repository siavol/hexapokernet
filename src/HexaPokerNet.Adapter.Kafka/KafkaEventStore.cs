using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using HexaPokerNet.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

public class KafkaEventStore : IEventStore
{
    private readonly ProducerBuilder<string, string> _producerBuilder;

    public KafkaEventStore(IKafkaConfiguration configuration, ILogger<KafkaEventStore> logger)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = configuration.KafkaServer,
            EnableDeliveryReports = true
        };
        _producerBuilder = new ProducerBuilder<string, string>(producerConfig);

        logger.LogDebug("Kafka event store created for {0}", configuration.KafkaServer);
    }

    public async Task RegisterEvent(IEntityEvent entityEvent)
    {
        if (entityEvent == null) throw new ArgumentNullException(nameof(entityEvent));

        var eventJson = EntityEventSerializer.Serialize(entityEvent);
        using var producer = _producerBuilder.Build();
        await producer.ProduceAsync(KafkaTopic.EntityEvents,
            new Message<string, string> { Key = entityEvent.EntityKey, Value = eventJson }
        );
    }
}