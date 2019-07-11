using System;
using SwarmSim.Classes.Entities;

namespace SwarmSim
{
    public class Frame
    {
        Space[,] _map;
        public Space[,] map => _map;
        public Random _rng;
        public Frame(int xSize = 10, int ySize = 10, int drones = 3)
        {
            _rng = new Random();
            _map = new Space[xSize,ySize];

            _map = CreateMaze(_map);

            _map = AddDrones(_map, drones);
        }


        private Space[,] CreateMaze(Space[,] map)
        {
            for (int x = 0; x < map.GetLength(0); x += 1) 
            {
                for (int y = 0; y < map.GetLength(1); y += 1) 
                {
                    var rand = _rng.NextDouble();
                    if (rand < 0.1)
                    {
                        map[x, y] = new Wall();
                    } else
                    {
                        map[x, y] = new Unexplored();
                    }
                }
            }

            return map;
        }

        private Space[,] AddDrones(Space[,] map, int drones)
        {
            int xCoord;
            int yCoord;

            for (int i = 0; i < drones; i++)
            {
                do
                {
                    xCoord = _rng.Next(0,map.GetLength(0));
                    yCoord = _rng.Next(0,map.GetLength(1));
                } while (map[xCoord,yCoord].IsSolid == true);

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