using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Drone : Space
    {
        DroneGroupState GroupState = DroneGroupState.Ungrouped;
        DroneActionState ActionState = DroneActionState.Exploring;
        public EntityType Type = EntityType.Drone;

        public bool IsSolid = true;

        public override string ToString()
        {
            return "X";
        }
    }
}