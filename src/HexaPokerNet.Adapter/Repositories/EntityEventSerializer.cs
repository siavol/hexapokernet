using System.Diagnostics.CodeAnalysis;
using Confluent.Kafka;
using HexaPokerNet.Application.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HexaPokerNet.Adapter.Repositories;

public static class EntityEventSerializer
{
    public static string Serialize<T>(T entityEvent) where T : IEntityEvent
    {
        var settings = GetJsonSerializerSettings<T>();
        var json = JsonConvert.SerializeObject(entityEvent, settings);
        return json;
    }

    public static T Deserialize<T>(string json) where T : IEntityEvent
    {
        var settings = GetJsonSerializerSettings<T>();
        var result = JsonConvert.DeserializeObject<T>(json, settings);
        if (result != null)
        {
            return result;
        }
        else
        {
            throw new InvalidOperationException("Can not deserialize JSON");
        }
    }

    private static JsonSerializerSettings GetJsonSerializerSettings<T>() where T : IEntityEvent
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            SerializationBinder = new EventTypeSerializationBinder()
        };
        return settings;
    }

    private class EventTypeSerializationBinder : ISerializationBinder
    {
        public Type BindToType(string? assemblyName, string typeName)
        {
            return typeName switch
            {
                "StoryAddedEvent" => typeof(StoryAddedEvent),
                _ => throw new InvalidOperationException($"Can not deserialize unknown type '${typeName}'")
            };
        }

        public void BindToName(Type serializedType, [UnscopedRef] out string? assemblyName, [UnscopedRef] out string? typeName)
        {
            if (serializedType == typeof(StoryAddedEvent))
            {
                assemblyName = null;
                typeName = "StoryAddedEvent";
            }
            else
            {
                throw new NotSupportedException($"Type {serializedType} is not supported");
            }
        }
    }
}