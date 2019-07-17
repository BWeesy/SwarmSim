using System;
using System.Collections.Generic;
using SwarmSim.Enums;
using SwarmSim.Interfaces;

namespace SwarmSim.Classes.Entities
{
    public class Drone : Space
    {
        public EntityType State = EntityType.UngroupedDrone;

        public override bool IsSolid() => true;

        public override string ToString()
        {
            switch (State)
            {
                case EntityType.UngroupedDrone:
                    return "8";
                case EntityType.LeaderDrone:
                    return "0";
                case EntityType.SubordinateDrone:
                    return "O";
                default:
                    return "?";
            }
        }

        public static ISpace[,] UngroupedDroneMove(int x, int y, Drone drone, ISpace[,] previousMap)
        {
            var map = previousMap;
            //TODO Look for close Leaders with space in their group. If found, move towards them
            //Else, explore by moving towards closest unexplored space
            var targetSpace = FindRandomAdjacentUnexplored(x, y, map);

            if (targetSpace != null)
            {
                map = MoveDrone(x, y, targetSpace.Value.x, targetSpace.Value.y, map);
            }

            return map;
        }

        public static ISpace[,] LeaderDroneMove(int x, int y, Drone drone, ISpace[,] previousMap)
        {
            var map = previousMap;
            //Explore by moving towards closest unexplored space
            return map;
        }

        public static ISpace[,] SubordinateDroneMove(int x, int y, Drone drone, ISpace[,] previousMap)
        {
            var map = previousMap;
            //Follow the leader
            return map;
        }

        private static ISpace[,] MoveDrone(int currentX, int currentY, int targetX, int targetY, ISpace[,] previousMap)
        {
            var map = previousMap;
            //Validate currentCoords are actually a drone
            if (!(map[currentX, currentY] is Drone))
            {
               throw new Exception($"Failed to move drone from {map[currentX, currentY]} as it is not a drone");
            }
            //Validate target is not solid
            if (map[targetX, targetY].IsSolid())
            {
               throw new Exception($"Failed to move drone into {map[targetX, targetY]} as that space is solid");
            }

            //Move drone into the target space
            map[targetX, targetY] = map[currentX, currentY];

            //Change old space to an Explored space
            map[currentX, currentY] = new Explored();

            return map;
        }

        private static (int x, int y)? FindRandomAdjacentUnexplored(int x, int y, ISpace[,] map)
        {
            var neighbours = new List<(int x, int y)>();
            var rng = new Random();

            //x+ve direction
            if(x < map.GetLength(0) && !map[x+1,y].IsSolid())
            {
                neighbours.Add((x+1,y));
            }

            //x-ve direction
            if(x>0 && !map[x-1,y].IsSolid())
            {
                neighbours.Add((x-1,y));
            }

            //y+ve direction
            if(y < map.GetLength(1) && !map[x,y+1].IsSolid())
            {
                neighbours.Add((x,y+1));
            }

            //y-ve direction
            if(y>0 && !map[x,y-1].IsSolid())
            {
                neighbours.Add((x,y-1));
            }

            if (neighbours.Count == 0) return null;

            return neighbours[rng.Next(neighbours.Count)];

        }
        //TODO Patchfinding method
        //TODO Explore method
            //Look for closest unexplored
            //Pathfind to the closest unexplored
            //Make first move along that path
    }
}