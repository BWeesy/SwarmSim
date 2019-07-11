using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Unexplored : Space
    {
        public override bool IsSolid() => false;
        public override EntityType Type() => EntityType.Unexplored;

        public override string ToString()
        {
        return "*";
        }
    }
}