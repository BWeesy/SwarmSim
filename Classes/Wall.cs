using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Wall : Space
    {
        public override bool IsSolid() => true;
        public override EntityType Type() => EntityType.Wall;

        public override string ToString()
        {
        return "█";
        }
    }
}