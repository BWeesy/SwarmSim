using System;
using SwarmSim.Classes.Entities;
using SwarmSim.Interfaces;
using System.Collections.Generic;

namespace SwarmSim
{
    public class Frame
    {
        ISpace[,] _map;
        public ISpace[,] map => _map;
        public Random _rng;
        public Frame(int xSize = 100, int ySize = 100, int drones = 3)
        {
            _rng = new Random();
            _map = new ISpace[xSize,ySize];
            _map = InitialiseMapToWalls(_map);
            _map = MapGenStep(_map, _rng.Next(xSize), _rng.Next(ySize));

            //Validate generated map

            _map = AddDrones(_map, drones);
        }

        private ISpace[,] MapGenStep (ISpace[,] originalMap, int currentX, int currentY)
        {
            var map = originalMap;
            var neighbours = FindCandidateNeighbours(map, currentX, currentY);

            while (neighbours.Count > 0)
            {
                var cutTo = neighbours[_rng.Next(neighbours.Count)];

                map[cutTo.targetX, cutTo.targetY] = new Unexplored();
                map[cutTo.wallX, cutTo.wallY] = new Unexplored();
                map = MapGenStep(map, cutTo.targetX, cutTo.targetY);

                neighbours = FindCandidateNeighbours(map, currentX, currentY);
            }

            return map;
        }

        private List<(int targetX, int targetY, int wallX, int wallY)> FindCandidateNeighbours (ISpace[,] originalMap, int x, int y)
        {
            var neighbours = new List<(int targetX, int targetY, int wallX, int wallY)>();
            //Edge checking

            //x+ve
            if(x < map.GetLength(0)-2 && map[x+1,y] is Wall && map[x+2,y] is Wall)
            {
                neighbours.Add((x+2,y,x+1,y));
            }
            //x-ve
            if(x>1 && map[x-1,y] is Wall && map[x-2,y] is Wall)
            {
                neighbours.Add((x-2,y,x-1,y));
            }
            //y+ve
            if(y < map.GetLength(1)-2 && map[x,y+1] is Wall && map[x,y+2] is Wall)
            {
                neighbours.Add((x,y+2,x,y+1));
            }
            //y-ve
            if(y>1 && map[x,y-1] is Wall && map[x,y-2] is Wall)
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
    }
}