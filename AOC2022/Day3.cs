using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day3
    {
        string[] inp;

        public Day3()
        {
            inp = InputHandler.GetInput(3).Split();
        }

        private int Score(char c)
        {
            if ('a' <= c && c <= 'z') return c - 'a' + 1;
            if ('A' <= c && c <= 'Z') return c - 'A' + 27;
            return -1;
        }

        public int Part1()
        {
            return inp.Sum(line =>
            {
                int n = line.Length / 2;
                var shared = line.Substring(0, n).Intersect(line.Substring(n));
                return Score(shared.First());
            });
        }

        public int Part2()
        {
            int sum = 0;
            for(int i = 0; i < inp.Length; i += 3)
            {
                IEnumerable<string> group = inp.Skip(i).Take(3);
                sum += Score(Utils.Intersect(group).First());
            }
            return sum;
        }
    }
}