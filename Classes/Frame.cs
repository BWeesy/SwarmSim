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
        public Frame(int xSize = 10, int ySize = 10, int drones = 3)
        {
            _rng = new Random();
            _map = new ISpace[xSize,ySize];

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

                //Make cut to neighbour, call generation step on that neightbour
                //Set working version of the returned map

                neighbours = FindCandidateNeighbours(map, currentX, currentY);
            }

            return map;
        }

        private List<(int x, int y)> FindCandidateNeighbours (ISpace[,] originalMap, int x, int y)
        {
            var neighbours = new List<(int x, int y)>();
            //Edge checking

            //x+ve
            if(map[x+1,y] is null && map[x+2,y] is null)
            {
                neighbours.Add((x+2,y));
            }
            //x-ve
            if(map[x-1,y] is null && map[x-2,y] is null)
            {
                neighbours.Add((x-2,y));
            }
            //y+ve
            if(map[x,y+1] is null && map[x,y+2] is null)
            {
                neighbours.Add((x,y+2));
            }
            //y-ve
            if(map[x,y-1] is null && map[x,y-2] is null)
            {
                neighbours.Add((x,y-2));
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