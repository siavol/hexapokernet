using System.Text;
using Confluent.Kafka;
using HexaPokerNet.Application.Events;

namespace HexaPokerNet.Adapter.Repositories.Kafka;

internal class EntityEventKafkaDeserializer : IDeserializer<IEntityEvent>
{
    public IEntityEvent Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            throw new NotSupportedException(
                "EntityEventKafkaDeserializer does not support deserialization to deserialize when isNull == true. This needs to be implemented.");
        }

        var json = Encoding.UTF8.GetString(data);
        return EntityEventSerializer.Deserialize<IEntityEvent>(json);
    }
}