using SwarmSim.Enums;

namespace SwarmSim.Interfaces
{
    public interface ISpace
    {       
        bool IsSolid();
        EntityType Type();
    }
}