using SwarmSim.Enums;

namespace SwarmSim.Classes.Entities
{
    public class Drone : Space
    {
        EntityType _state = EntityType.UngroupedDrone;

        public override bool IsSolid() => true;
        public override EntityType Type() => _state;

        public override string ToString()
        {
            switch (_state)
            {
                case EntityType.UngroupedDrone:
                    return "X";
                case EntityType.LeaderDrone:
                    return "0";
                case EntityType.SubordinateDrone:
                    return "O";
                default:
                    return "?";
            }
        }
    }
}