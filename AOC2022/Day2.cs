using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day2
    {
        (int opp, int you)[] inp;

        public Day2()
        {
            string[][] tmp = InputHandler.GetInput(2).SplitTwice('\n', ' ');
            inp = tmp.Map(arr => (arr[0][0] - 'A', arr[1][0] - 'X'));
        }

        public int Part1()
        {
            int[] scores = new int[] { 4, 1, 7 };
            return inp.Sum(p => p.you + scores[Utils.Mod(p.opp - p.you, 3)]);
        }

        public int Part2()
            => inp.Sum(p => p.you * 3 + Utils.Mod(p.opp + p.you + 2, 3) + 1);
    }
}