using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Unexplored : Space
    {
        public EntityType State = EntityType.Unexplored;

        public override bool IsSolid() => false;

        public override string ToString()
        {
        return "X";
        }
    }
}