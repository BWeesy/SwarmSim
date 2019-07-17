using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Explored : Space
    {
        public EntityType State = EntityType.Explored;

        public override bool IsSolid() => false;

        public override string ToString()
        {
        return "·";
        }
    }
}