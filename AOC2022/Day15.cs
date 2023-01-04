using System;
using System.Linq;

namespace AOC2022
{
    public class Day15
    {
        int[][] inp;

        public Day15()
        {
            string[] file = InputHandler.GetInput(15).Split('\n');
            inp = file.Map(s => s.Replace("Sensor at x=", "").Replace(", y=", " ")
                .Replace(": closest beacon is at x=", " ").Replace(", y=", " ").SplitInts(' '));
        }

        public int Part1()
        {
            (int s, int e) = GetRanges(2000000, int.MinValue, int.MaxValue)[0];
            return e - s;
        }

        private List<(int, int)> MergeRanges(List<(int, int)> ranges)
        {
            ranges = ranges.OrderBy(r => r.Item1).ToList();
            for (int i = 0; i < ranges.Count; i++)
            {
                (int s, int e) t = ranges[i];
                for (int j = i + 1; j < ranges.Count; j++)
                {
                    (int s, int e) r = ranges[j];
                    if (t.s <= r.e + 1 && r.s <= t.e + 1)
                    {
                        t = (Math.Min(t.s, r.s), Math.Max(t.e, r.e));
                        ranges.RemoveAt(j);
                        j--;
                    }
                }
                ranges[i] = t;
            }
            return ranges;
        }

        List<(int, int)> GetRanges(int y, int minCoord, int maxCoord)
        {
            List<(int, int)> ranges = new List<(int, int)>();
            foreach (int[] line in inp)
            {
                (int sx, int sy, int bx, int by) = (line[0], line[1], line[2], line[3]);

                int dist = Math.Abs(bx - sx) + Math.Abs(by - sy);
                int dx = dist - Math.Abs(y - sy);

                (int s, int e) = (Math.Max(minCoord, sx - dx), Math.Min(maxCoord, sx + dx));
                if (e >= s) ranges.Add((s, e));
            }
            return MergeRanges(ranges);
        }

        public long Part2()
        {
            int maxCoord = 4000000;
            for (int y = 0; y < maxCoord; y++)
            {
                List<(int, int)> ranges = GetRanges(y, 0, maxCoord);
                if (ranges.Count() > 1)
                    return ((long)ranges[0].Item2 + 1) * maxCoord + y;
            }
            return -1;
        }
    }
}