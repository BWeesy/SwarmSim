using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Wall : Space
    {
        public bool IsSolid = true;
        public EntityType Type = EntityType.Wall;

        public override string ToString()
        {
        return "█";
        }
    }
}