using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day6
    {
        string input;

        public Day6()
        {
            input = InputHandler.GetInput(6);
        }

        private int GetMarker(int n)
        {
            Queue<char> prev = new Queue<char>(input.Substring(0, n));
            if (prev.Count == prev.Distinct().Count()) return n;

            for (int i = n; i < input.Length; i++)
            {
                prev.Dequeue();
                prev.Enqueue(input[i]);
                if (prev.Count == prev.Distinct().Count()) return i + 1;
            }
            return -1;
        }

        public int Part1() => GetMarker(4);

        public int Part2() => GetMarker(14);
    }
}
