using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Drone : Space
    {
        DroneGroupState GroupState = DroneGroupState.Ungrouped;
        DroneActionState ActionState = DroneActionState.Exploring;

        public override bool IsSolid() => true;
        public override EntityType Type() => EntityType.Drone;

        public override string ToString()
        {
            return "X";
        }
    }
}