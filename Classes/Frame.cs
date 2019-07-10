using System;

namespace SwarmSim
{
    public class Frame
    {
        Space[,] map;

        public Frame()
        {
            var rng = new Random();
            map = new Space[10,10];
            for (int x = 0; x < map.GetLength(0); x += 1) 
            {
                for (int y = 0; y < map.GetLength(1); y += 1) 
                {
                    var rand = rng.NextDouble();
                    if (rand < 0.1)
                    {
                        map[x, y] = new Wall();
                    } else
                    {
                        map[x, y] = new Unexplored();
                    }
                }
            }
        }

        public override string ToString()
        {
            string printedMap = "";
            for (int x = 0; x < map.GetLength(0); x += 1) 
            {
                for (int y = 0; y < map.GetLength(1); y += 1) 
                {
                    printedMap = printedMap+map[x, y].ToString();
                }   
                printedMap = printedMap+System.Environment.NewLine;
            }
            return printedMap;
        }
    }
}