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

        public static ISpace[,] UngroupedDroneMove(int currentX, int currentY, Drone drone, ISpace[,] previousMap)
        {
            var map = previousMap;
            //Explore by moving towards closest unexplored space, prefering the least recently explored space

            var neighbours = GetNeighbours(currentX, currentY, map);   

            var targetSpace = Explore(neighbours);

            if (targetSpace != null)
            {
                map = MoveDrone(currentX, currentY, targetSpace.Value.x, targetSpace.Value.y, map);
            }

            return map;
        }

        public static ISpace[,] PredatorDroneMove(int currentX, int currentY, Drone drone, ISpace[,] previousMap)
        {
            var map = previousMap;

            var neighbours = GetNeighbours(currentX, currentY, map);   
            (int x, int y)? targetSpace = null;

            //targetSpace = Kill(neighbours, map);

            if(targetSpace == null){
                targetSpace = Hunt(neighbours, map);
            }
            if(targetSpace == null){
                targetSpace = Explore(neighbours);
            }

            if (targetSpace != null)
            {
                map = MoveDrone(currentX, currentY, targetSpace.Value.x, targetSpace.Value.y, map);
            }

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
            var targetDrone = map[targetX, targetY] as Drone;
            var currentDrone = map[currentX, currentY] as Drone;
            if(targetDrone != null){
                var temp = map[targetX, targetY];
                map[currentX, currentY] = map[targetX, targetY];
                map[targetX, targetY] = temp;
            } else {
                map[targetX, targetY] = map[currentX, currentY];
                map[currentX, currentY] = new Explored(currentDrone.State == EntityType.UngroupedDrone ? 100 : 0);
            }

            return map;
        }

        private static (int x, int y)? Explore(Neighbours neighbours)
        {   
            if(neighbours.Unexplored.Count + neighbours.Explored.Count == 0){
                return null;
            }

            var rng = new Random();            
            return neighbours.Unexplored.Count > 0
            ? neighbours.Unexplored[rng.Next(neighbours.Unexplored.Count)]
            : neighbours.Explored.OrderBy(e => e.activity).Select(e => (e.x, e.y)).FirstOrDefault();
        }
        
        private static (int x, int y)? Hunt(Neighbours neighbours, ISpace[,] map)
        {   
            var rng = new Random();

            var activeNeighbours = neighbours.Explored.Where(e => e.activity > 0).OrderByDescending(e => e.activity);
            if(activeNeighbours.Count() > 0) return activeNeighbours.Select(e => (e.x, e.y)).FirstOrDefault();
            return null;
        }

        private static Neighbours GetNeighbours(int x, int y, ISpace[,] map){
            var unexploredNeighbours = new List<(int x, int y)>();
            var exploredNeighbours = new List<(int x, int y, int activity)>();
            var droneNeighbours = new List<(int x, int y, Drone drone)>();

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
                if (map[x+1, y] is Drone)
                {
                    var drone = (Drone) map[x+1, y];            
                    droneNeighbours.Add((x+1, y, drone));
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
                if (map[x-1, y] is Drone)
                {
                    var drone = (Drone) map[x-1, y];            
                    droneNeighbours.Add((x-1, y, drone));
                }   
            }

            //y+ve direction
            if(y < map.GetLength(1) - 1 && !map[x, y+1].IsSolid())
            {
                if (map[x, y+1] is Unexplored)
                {
                    unexploredNeighbours.Add((x, y+1));
                }
                if (map[x, y+1] is Explored)
                {
                    var explored = (Explored) map[x, y+1]; 
                    exploredNeighbours.Add((x, y+1, explored.Activity));
                } 
                if (map[x, y+1] is Drone)
                {
                    var drone = (Drone) map[x,y+1];            
                    droneNeighbours.Add((x,y+1, drone));
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
                if (map[x, y-1] is Drone)
                {
                    var drone = (Drone) map[x, y-1];            
                    droneNeighbours.Add((x, y-1, drone));
                }
            }

            return new Neighbours{
                Unexplored = unexploredNeighbours,
                Explored = exploredNeighbours,
                Drone = droneNeighbours
            };
        }


        //TODO Patchfinding method
        //TODO Explore method
            //Look for closest unexplored
            //Pathfind to the closest unexplored
            //Make first move along that path
    }
}