using System.Diagnostics;

using AOC2022;

var day = new Day11();

Stopwatch stopwatch = Stopwatch.StartNew();
var res = day.Part1();
stopwatch.Stop();
Console.WriteLine($"Part1: {res}\nFound in {stopwatch.ElapsedMilliseconds}ms\n-----\n");

stopwatch = Stopwatch.StartNew();
var res2 = day.Part2();
stopwatch.Stop();
Console.WriteLine($"Part2: {res2}\nFound in {stopwatch.ElapsedMilliseconds}ms");

/* Template
using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class DayX
    {
        public DayX()
        {
            string[] tmp = InputHandler.GetInput(X).Split();
        }

        public int Part1()
        {
            return -1;
        }

        public int Part2()
        {
            return -1;
        }
    }
}
 */