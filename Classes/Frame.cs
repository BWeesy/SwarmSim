using System;
using System.Collections.Generic;
using SwarmSim.Classes.Entities;
using SwarmSim.Interfaces;
using SwarmSim.Enums;
using System.Linq;

namespace SwarmSim
{
    public class Frame
    {
        ISpace[,] _map;
        public ISpace[,] map => _map;
        public Random _rng;
        public Frame(int xSize = 25, int ySize = 25, ISpace[,] map = null)
        {
            _rng = new Random();
            if (map == null)
            {
                _map = new ISpace[xSize,ySize];
            } else
            {
                _map = map;
            }
        }

        public void Init(int drones = 3)
        {
            do
            {
            _map = InitialiseMapToWalls(_map);
            _map = MapGenStep(_map, _rng.Next(map.GetLength(0)), _rng.Next(map.GetLength(1)));
            _map = AddDrones(_map, drones);
            } while (!IsValidMap(_map));
        }

        private ISpace[,] MapGenStep (ISpace[,] originalMap, int currentX, int currentY)
        {
            var map = originalMap;
            var validCandidates = FindCandidateNeighbours(map, currentX, currentY);

            while (validCandidates.Count > 0)
            {
                var cutTo = validCandidates[_rng.Next(validCandidates.Count)];

                map[cutTo.targetX, cutTo.targetY] = new Unexplored();
                map[cutTo.wallX, cutTo.wallY] = new Unexplored();
                map = MapGenStep(map, cutTo.targetX, cutTo.targetY);

                validCandidates = FindCandidateNeighbours(map, currentX, currentY);
            }

            return map;
        }

        private List<(int targetX, int targetY, int wallX, int wallY)> FindCandidateNeighbours (ISpace[,] originalMap, int x, int y)
        {
            var neighbours = new List<(int targetX, int targetY, int wallX, int wallY)>();

            //x+ve direction
            if(x < map.GetLength(0)-3 && map[x+1,y] is Wall && map[x+2,y] is Wall)
            {
                neighbours.Add((x+2,y,x+1,y));
            }

            //x-ve direction
            if(x>2 && map[x-1,y] is Wall && map[x-2,y] is Wall)
            {
                neighbours.Add((x-2,y,x-1,y));
            }

            //y+ve direction
            if(y < map.GetLength(1)-3 && map[x,y+1] is Wall && map[x,y+2] is Wall)
            {
                neighbours.Add((x,y+2,x,y+1));
            }

            //y-ve direction
            if(y>2 && map[x,y-1] is Wall && map[x,y-2] is Wall)
            {
                neighbours.Add((x,y-2,x,y-1));
            }

            return neighbours;
        }

        private ISpace[,] AddDrones(ISpace[,] map, int drones)
        {
            int xCoord;
            int yCoord;

            for (int i = 0; i < drones; i++)
            {
                do
                {
                    xCoord = _rng.Next(0,map.GetLength(0));
                    yCoord = _rng.Next(0,map.GetLength(1));
                } while (map[xCoord,yCoord].IsSolid());

                map[xCoord,yCoord] = new Drone();
            }

            return map;
        }

        private static List<(int x, int y, Drone drone)> FindDrones(ISpace[,] map)
        {
            // IndexOf and FindIndex are only 1 dimensional...
            var drones = new List<(int x, int y, Drone drone)>();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    if(map[i,j] is Drone)
                    {
                        drones.Add((i,j,(Drone) map[i,j]));
                    }
                }
            }
            return drones;
        }

        private ISpace[,] InitialiseMapToWalls (ISpace[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    map[i,j] = new Wall();
                }
            }
            return map;
        }

        public override string ToString()
        {
            string printedMap = "";
            for (int x = 0; x < _map.GetLength(0); x += 1) 
            {
                for (int y = 0; y < _map.GetLength(1); y += 1) 
                {
                    printedMap = printedMap+_map[x, y].ToString();
                }   
                printedMap = printedMap+System.Environment.NewLine;
            }
            return printedMap;
        }

        public static bool IsValidMap(ISpace[,] map)
        {
            //Check map has length in both dimensions
            if (map.GetLength(0) == 0) return false;
            if (map.GetLength(1) == 0) return false;

            //null check each space
            foreach(var space in map)
            {
                if(space == null) return false;
            }

            return true;
        }

        public static ISpace[,] NextStep(ISpace[,] previousMap)
        {
            //Initialise working map
            var map = previousMap;

            //Find drones
            var drones = FindDrones(map);

            var sortedDrones = drones.Where(x => x.drone.State == EntityType.UngroupedDrone)
            .Concat(drones.Where(x => x.drone.State == EntityType.LeaderDrone))
            .Concat(drones.Where(x => x.drone.State == EntityType.SubordinateDrone));

            //Apply the movement of each drone in turn
            foreach (var drone in sortedDrones)
            {
                //Update map with new position
                switch (drone.drone.State)
                {
                    case EntityType.UngroupedDrone:
                        map = Drone.UngroupedDroneMove(drone.x, drone.y, drone.drone, map);
                        break;
                    case EntityType.LeaderDrone:
                        map = Drone.LeaderDroneMove(drone.x, drone.y, drone.drone, map);
                        break;
                    case EntityType.SubordinateDrone:
                        map = Drone.SubordinateDroneMove(drone.x, drone.y, drone.drone, map);
                        break;
                    default:
                        break;
                }
            }
            return map;
        }
    }
}