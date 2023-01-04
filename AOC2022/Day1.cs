using System;
using System.Linq;

namespace AOC2022
{
    public class Day1
    {
        int[][] inp;

        public Day1()
        {
            inp = InputHandler.GetInput(1).SplitTwiceInts("\n\n", "\n");
        }

        public int Part1() => inp.Max(Enumerable.Sum);

        public int Part2()
            => inp.Map(Enumerable.Sum).OrderDescending().Take(3).Sum();
    }
}