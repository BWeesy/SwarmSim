using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Unexplored : Space
    {
        public bool IsSolid = false;
        public EntityType Type = EntityType.Unexplored;

        public override string ToString()
        {
        return "*";
        }
    }
}