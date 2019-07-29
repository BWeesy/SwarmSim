using System;
using System.Collections.Generic;
using SwarmSim.Enums;
using SwarmSim.Interfaces;
using System.Linq;

namespace SwarmSim.Classes.Entities
{
    public class Drone : Space
    {
        public EntityType State = EntityType.UngroupedDrone;
        public int Activity = 0;

        public override bool IsSolid() => false;


        public Drone(EntityType state = EntityType.UngroupedDrone, int activity = 0)
        {
            State = state;
            Activity = activity;
        }

        public override string ToString()
        {
            switch (State)
            {
                case EntityType.UngroupedDrone:
                    return "8";
                case EntityType.PredatorDrone:
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
            //Explore by moving towards closest unexplored space, prefering the least recently explored space

            var neighbours = GetNeighbours(x, y, map);   

            var targetSpace = Explore(neighbours);

            if (targetSpace != null)
            {
                map = MoveDrone(x, y, targetSpace.Value.x, targetSpace.Value.y, map);
            }

            return map;
        }

        public static ISpace[,] PredatorDroneMove(int x, int y, Drone drone, ISpace[,] previousMap)
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

            //Move drone into the target space, swapping with a drone if moving into it's space
            if(map[targetX, targetY] is Drone){
                var temp = map[targetX, targetY];
                map[currentX, currentY] = map[targetX, targetY];
                map[targetX, targetY] = temp;
            } else {
                map[targetX, targetY] = map[currentX, currentY];
                map[currentX, currentY] = new Explored(100);
            }

            return map;
        }

        private static (int x, int y)? Explore((List<(int x, int y)> unexploredNeighbours, List<(int x, int y, int activity)> exploredNeighbours) neighbours)
        {   
            var unexploredNeighbours = neighbours.unexploredNeighbours;
            var exploredNeighbours = neighbours.exploredNeighbours;

            var rng = new Random();            
            return unexploredNeighbours.Count > 0
            ? unexploredNeighbours[rng.Next(unexploredNeighbours.Count)]
            : exploredNeighbours.OrderBy(e => e.activity).Select(e => (e.x, e.y)).FirstOrDefault();
        }

        private static (List<(int x, int y)> unexploredNeighbours, List<(int x, int y, int activity)> exploredNeighbours) GetNeighbours(int x, int y, ISpace[,] map){
            var unexploredNeighbours = new List<(int x, int y)>();
            var exploredNeighbours = new List<(int x, int y, int activity)>();

            //x+ve direction
            if(x < map.GetLength(0) - 1 && !map[x+1, y].IsSolid())
            {
                if (map[x+1, y] is Unexplored)
                {
                    unexploredNeighbours.Add((x+1, y));
                }
                if (map[x+1, y] is Explored)
                {
                    var explored = (Explored) map[x+1, y];            
                    exploredNeighbours.Add((x+1, y, explored.Activity));
                }                
            }

            //x-ve direction
            if(x > 0 && !map[x-1, y].IsSolid())
            {
                if (map[x-1, y] is Unexplored)
                {
                    unexploredNeighbours.Add((x-1, y));
                }
                if (map[x-1, y] is Explored)
                {
                    var explored = (Explored) map[x-1, y];   
                    exploredNeighbours.Add((x-1, y, explored.Activity));
                }   
            }

            //y+ve direction
            if(y < map.GetLength(1) - 1 && !map[x, y+1].IsSolid())
            {
                if (map[x, y+1] is Unexplored)
                {
                    unexploredNeighbours.Add((x, y+1));
                }
                if (map[x,y+1] is Explored)
                {
                    var explored = (Explored) map[x, y+1]; 
                    exploredNeighbours.Add((x, y+1, explored.Activity));
                } 
            }

            //y-ve direction
            if(y > 0 && !map[x, y-1].IsSolid())
            {
                if (map[x, y-1] is Unexplored)
                {
                    unexploredNeighbours.Add((x, y-1));
                }
                if (map[x, y-1] is Explored)
                {
                    var explored = (Explored) map[x, y-1]; 
                    exploredNeighbours.Add((x, y-1, explored.Activity));
                } 
            }

            return (unexploredNeighbours, exploredNeighbours);
        }


        //TODO Patchfinding method
        //TODO Explore method
            //Look for closest unexplored
            //Pathfind to the closest unexplored
            //Make first move along that path
    }
}