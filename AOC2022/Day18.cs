using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day18
    {
        int[][] inp;
        HashSet<(int, int, int)> droplets;
        (int, int, int)[] sides = new (int, int, int)[]
                { (-1, 0, 0), (1, 0, 0), (0, -1, 0), (0, 1, 0), (0, 0, -1), (0, 0, 1) };

        public Day18()
        {
            inp = InputHandler.GetInput(18).SplitTwiceInts('\n', ',');
            droplets = new HashSet<(int, int, int)>(inp.Select(l => (l[0], l[1], l[2])));
        }

        public int Part1()
        {
            int count = 0;

            foreach((int x, int y, int z) drop in droplets)
            {
                foreach((int x, int y, int z) side in sides)
                {
                    if (!droplets.Contains((drop.x + side.x, drop.y + side.y, drop.z + side.z)))
                        count++;
                }
            }

            return count;
        }

        public int Part2()
        {
            int minX = droplets.Select(d => d.Item1).Min() - 1;
            int maxX = droplets.Select(d => d.Item1).Max() + 1;
            int minY = droplets.Select(d => d.Item2).Min() - 1;
            int maxY = droplets.Select(d => d.Item2).Max() + 1;
            int minZ = droplets.Select(d => d.Item3).Min() - 1;
            int maxZ = droplets.Select(d => d.Item3).Max() + 1;

            HashSet<(int, int, int)> reachable = new HashSet<(int, int, int)>();
            Queue<(int, int, int)> toVisit = new Queue<(int, int, int)>();
            toVisit.Enqueue((minX, minY, minZ));
            reachable.Add((minX, minY, minZ));

            while(toVisit.Count > 0)
            {
                (int x, int y, int z) curr = toVisit.Dequeue();
                foreach ((int x, int y, int z) side in sides)
                {
                    (int x, int y, int z) target = (curr.x + side.x, curr.y + side.y, curr.z + side.z);
                    if (target.x >= minX && target.y >= minY && target.z >= minZ &&
                        target.x <= maxX && target.y <= maxY && target.z <= maxZ &&
                        reachable.Add(target) && !droplets.Contains(target))
                            toVisit.Enqueue(target);
                }
            }

            int count = 0;

            foreach ((int x, int y, int z) drop in droplets)
            {
                foreach ((int x, int y, int z) side in sides)
                {
                    (int x, int y, int z) target = (drop.x + side.x, drop.y + side.y, drop.z + side.z);
                    if (!droplets.Contains(target) && reachable.Contains(target))
                        count++;
                }
            }

            return count;
        }
    }
}