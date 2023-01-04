using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day16
    {
        (int, int[])[] inp;
        Dictionary<string, int> valveMap = new Dictionary<string, int>();

        public Day16()
        {
            string[] file = InputHandler.GetInput(16).Split('\n');
            (string, string[])[] split = new (string, string[])[file.Length];
            for(int i = 0; i < file.Length; i++)
            {
                string[] splitLine = file[i].Replace("s", "").Split("; tunnel lead to valve ");
                string[] targets = splitLine[1].Split(", ");
                string[] valveInfo = splitLine[0].Replace("rate=", "").Split();
                split[i] = (valveInfo[4], targets);
                valveMap[valveInfo[1]] = i;
            }

            inp = new (int, int[])[file.Length];
            for (int i = 0; i < file.Length; i++)
            {
                (string rate, string[] targets) = split[i];
                inp[i] = (int.Parse(rate), targets.Map(t => valveMap[t]));
            }
        }

        Dictionary<(long, int, int), int> cache = new Dictionary<(long, int, int), int>();

        private int Score(long openValves, int rate, int currValve, int depth)
        {
            // Depth 30, can't do anything
            if (depth == 13) return 0;

            var key = (openValves, currValve, depth);
            if (!cache.ContainsKey(key))
            {
                int max = 0;
                // If this valve isn't open, try opening it
                if ((openValves & ((long)1 << currValve)) == 0 && inp[currValve].Item1 != 0)
                {
                    max = Score(openValves | ((long)1 << currValve),
                        rate + inp[currValve].Item1,
                        currValve,
                        depth + 1);
                }
                // Or don't open it and just visit other valves
                foreach (int target in inp[currValve].Item2)
                    max = Math.Max(max, Score(openValves, rate, target, depth + 1));
                cache[key] = rate + max;
            }
            return cache[key];
        }

        public int Part1() => Score(0, 0, valveMap["AA"], 0);

        Dictionary<(long, int, int, int), int> cache2 = new Dictionary<(long, int, int, int), int>();

        private int Score2(long openValves, int rate, int currValve, int elephValve, int depth)
        {
            // Depth 26, can't do anything
            if (depth == 26) return 0;

            int v1 = elephValve, v2 = currValve;
            if (v2 > v1) (v1, v2) = (v2, v1);
            var key = (openValves, v1, v2, depth);
            if (!cache2.ContainsKey(key))
            {
                int max = 0;
                // If this valve isn't open, try opening it
                if ((openValves & ((long)1 << currValve)) == 0 && inp[currValve].Item1 != 0)
                {
                    // The elephant tries the same thing
                    if ((openValves & ((long)1 << elephValve)) == 0 && inp[elephValve].Item1 != 0 && elephValve != currValve)
                        max = Score2(openValves | ((long)1 << currValve) | ((long)1 << elephValve),
                            rate + inp[currValve].Item1 + inp[elephValve].Item1,
                            currValve,
                            elephValve,
                            depth + 1);
                    else
                    {
                        foreach (int target in inp[elephValve].Item2)
                        {
                            max = Math.Max(max, Score2(openValves | ((long)1 << currValve),
                                rate + inp[currValve].Item1,
                                currValve,
                                target,
                                depth + 1));
                        }
                    }
                }
                // Or don't open it and just visit other valves
                foreach (int target in inp[currValve].Item2)
                {
                    // The elephant tries the same thing
                    if ((openValves & ((long)1 << elephValve)) == 0 && inp[elephValve].Item1 != 0)
                        max = Math.Max(max, Score2(openValves | ((long)1 << elephValve),
                            rate + inp[elephValve].Item1,
                            target,
                            elephValve,
                            depth + 1));
                    else
                    {
                        foreach (int target2 in inp[elephValve].Item2)
                            max = Math.Max(max, Score2(openValves, rate, target, target2, depth + 1));
                    }
                }
                cache2[key] = rate + max;
            }
            return cache2[key];
        }


        public int Part2() => -1;
        //public int Part2() => Score2(0, 0, valveMap["AA"], valveMap["AA"], 0);
    }
}