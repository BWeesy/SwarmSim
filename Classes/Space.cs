using SwarmSim.Interfaces;
using SwarmSim.Enums;
using Newtonsoft.Json;

namespace SwarmSim.Classes.Entities
{
    public abstract class Space : ISpace
    {
        public override abstract string ToString();
        public abstract bool IsSolid();
        public abstract EntityType Type();
    }
}