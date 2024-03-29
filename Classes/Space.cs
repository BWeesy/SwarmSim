using System;
using SwarmSim.Interfaces;
using SwarmSim.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SwarmSim.Classes.Entities
{
    [JsonConverter(typeof(SpaceConverter))]
    public abstract class Space : ISpace
    {
        public override abstract string ToString();
        public abstract bool IsSolid();
    }

    public class SpaceConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Not implemented yet");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return string.Empty;
            } 
            else if (reader.TokenType == JsonToken.String)
            {
                return serializer.Deserialize(reader, objectType);
            }
            else
            {
                JObject obj = JObject.Load(reader);
                if(obj["State"] != null && Enum.TryParse<EntityType>(obj["State"].ToString(), out var value))
                {
                    int activity = 0;
                    if (obj["Activity"] != null && Int32.TryParse(obj["Activity"].ToString(), out var parsedActivity))
                    {
                        activity = parsedActivity;
                    }

                    switch (value)
                    {
                        case EntityType.Wall:
                            return new Wall();
                        case EntityType.Unexplored:
                            return new Unexplored();
                        case EntityType.Explored:
                            return new Explored(activity);
                        case EntityType.UngroupedDrone:
                            return new Drone();
                        case EntityType.PredatorDrone:
                            return new Drone(EntityType.PredatorDrone, activity);
                        case EntityType.SubordinateDrone:
                            return new Drone(EntityType.SubordinateDrone);
                        default:
                        throw new NotImplementedException("Not implemented yet");
                    }
                }
                else 
                    return serializer.Deserialize(reader, objectType);
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return false;
        }
    }
}