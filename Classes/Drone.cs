using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Drone : Space
    {
        DroneGroupState GroupState = DroneGroupState.Ungrouped;
        public bool IsSolid = true;    

        public override string ToString()
        {
            return "D";
        }
    }
}