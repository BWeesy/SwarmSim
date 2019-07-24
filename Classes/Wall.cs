using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Wall : Space
    {
        public EntityType State = EntityType.Wall;

        public override bool IsSolid() => true;

        public override string ToString()
        {
        return "█";
        }
    }
}