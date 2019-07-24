using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Explored : Space
    {
        public EntityType State = EntityType.Explored;
        public int Activity = 0;

        public Explored(int activity)
        {
            Activity = activity;
        }
        public override bool IsSolid() => false;

        public override string ToString()
        {
        return "·";
        }
    }
}