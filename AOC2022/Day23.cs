using System;
namespace AOC2022
{
    public class Day23
    {
        string[] inp;
        (int, int)[] elves;

        public Day23()
        {
            string file = InputHandler.GetInput(23);
            inp = file.Split();
            elves = new (int, int)[file.Count(c => c == '#')];
        }

        private void SetElves()
        {
            int ix = 0;
            for (int r = 0; r < inp.Length; r++)
            {
                string row = inp[r];
                for (int c = 0; c < row.Length; c++)
                {
                    if (row[c] == '#')
                        elves[ix++] = (r, c);
                }
            }
        }

        enum Dir { NW, N, NE, W, E, SW, S, SE };

        private static bool Check(bool[] hasElves, Dir d1, Dir d2, Dir d3)
            => !(hasElves[(int)d1] || hasElves[(int)d2] || hasElves[(int)d3]);

        Func<bool[], bool>[] checks = new Func<bool[], bool>[]
            {
                arr => Check(arr, Dir.NW, Dir.N, Dir.NE),
                arr => Check(arr, Dir.SW, Dir.S, Dir.SE),
                arr => Check(arr, Dir.NW, Dir.W, Dir.SW),
                arr => Check(arr, Dir.NE, Dir.E, Dir.SE)
            };

        Func<(int r, int c), (int, int)>[] actions = new Func<(int r, int c), (int, int)>[]
        {
                p => (p.r - 1, p.c),
                p => (p.r + 1, p.c),
                p => (p.r, p.c - 1),
                p => (p.r, p.c + 1)
        };

        private int Simulate(bool part2)
        {
            SetElves();
            (int r, int c)[] nextPos = new (int, int)[elves.Length];

            HashSet<(int, int)> proposed = new HashSet<(int, int)>();
            HashSet<(int, int)> multiple = new HashSet<(int, int)>();
            HashSet<(int, int)> occupied = new HashSet<(int, int)>(elves);

            for (int i = 0; part2 || i < 10; i++)
            {
                proposed.Clear();
                multiple.Clear();

                for (int elf = 0; elf < elves.Length; elf++)
                {
                    (int r, int c) pos = elves[elf];
                    nextPos[elf] = pos;

                    List<(int, int)> neighbours = new List<(int, int)>();
                    for (int dr = -1; dr <= 1; dr++)
                    {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            if (dr == 0 && dc == 0) continue;
                            neighbours.Add((pos.r + dr, pos.c + dc));
                        }
                    }

                    bool[] hasElves = neighbours.Select(occupied.Contains).ToArray();

                    if (hasElves.Any(b => b))
                    {
                        for (int ci = i; ci < i + checks.Length; ci++)
                        {
                            if (checks[ci % checks.Length](hasElves))
                            {
                                nextPos[elf] = actions[ci % checks.Length](pos);
                                if (!proposed.Add(nextPos[elf]))
                                    multiple.Add(nextPos[elf]);
                                break;
                            }
                        }
                    }
                }

                // i > 10 at this point, so won't run for part 1
                if (proposed.Count() == 0)
                    return i + 1;

                for (int elf = 0; elf < elves.Length; elf++)
                {
                    if (!multiple.Contains(nextPos[elf]))
                    {
                        occupied.Remove(elves[elf]);
                        elves[elf] = nextPos[elf];
                        occupied.Add(elves[elf]);
                    }
                }
            }

            // Should only be reachable if part2 is false
            int[] xs = elves.Map(e => e.Item1);
            int[] ys = elves.Map(e => e.Item2);

            return (xs.Max() - xs.Min() + 1) * (ys.Max() - ys.Min() + 1) - elves.Length;
        }

        public int Part1() => Simulate(false);

        public int Part2() => Simulate(true);
    }
}