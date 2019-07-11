using SwarmSim.Interfaces;
using SwarmSim.Enums;
using Newtonsoft.Json;
using SwarmSim.Utilities;

namespace SwarmSim.Classes.Entities
{
    [JsonConverter(typeof(EntityJSONConverter))]
    public abstract class Space : ISpace
    {
        public override abstract string ToString();
        public abstract bool IsSolid();
        public abstract EntityType Type();
    }
}