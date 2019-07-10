namespace SwarmSim.Classes.Entities
{
    public class Wall : Space
    {
        public bool IsSolid = true;
        public override string ToString()
        {
        return "W";
        }
    }
}