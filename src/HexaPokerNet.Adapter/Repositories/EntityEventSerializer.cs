using System.Diagnostics.CodeAnalysis;
using HexaPokerNet.Application.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HexaPokerNet.Adapter.Repositories;

public static class EntityEventSerializer
{
    public static string Serialize<T>(T entityEvent) where T : IEntityEvent
    {
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            SerializationBinder = new EventTypeSerializationBinder()
        };
        var json = JsonConvert.SerializeObject(entityEvent, settings);
        return json;
    }

    private class EventTypeSerializationBinder : ISerializationBinder
    {
        public Type BindToType(string? assemblyName, string typeName)
        {
            throw new NotImplementedException();
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