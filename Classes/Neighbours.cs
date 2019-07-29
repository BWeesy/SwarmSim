using System.Collections.Generic;

namespace SwarmSim.Classes.Entities
{
    struct Neighbours
    {
        public List<(int x, int y)> Unexplored;
        public List<(int x, int y, int activity)> Explored;
        public List<(int x, int y, Drone drone)> Drone;
    }
}