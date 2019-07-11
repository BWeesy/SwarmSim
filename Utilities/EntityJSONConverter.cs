using Newtonsoft.Json;
using System;
using SwarmSim.Classes.Entities;

namespace SwarmSim.Utilities
{
    public class EntityJSONConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                var space = value as Space;
                writer.WriteStartObject();
                writer.WritePropertyName("$" + "Type");
                serializer.Serialize(writer, space.Type());
                writer.WriteEndObject();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Space).IsAssignableFrom(objectType);
        }
    }
}