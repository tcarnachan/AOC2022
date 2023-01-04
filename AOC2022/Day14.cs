using System;
namespace AOC2022
{
    public class Day14
    {
        (int, int)[][] inp;
        int lowest;
        HashSet<(int x, int y)> filled = new HashSet<(int, int)>();

        public Day14()
        {
            string[][] file = InputHandler.GetInput(14).SplitTwice("\n", " -> ");
            inp = file.Map(line => line.Map(coord =>
            {
                string[] split = coord.Split(',');
                return (int.Parse(split[0]), int.Parse(split[1]));
            }));
        }

        private void FillRocks()
        {
            filled.Clear();
            foreach((int, int)[] rockStruct in inp)
            {
                (int x, int y) prev = rockStruct[0];
                for(int i = 1; i < rockStruct.Length; i++)
                {
                    filled.Add(prev);
                    (int x, int y) curr = rockStruct[i];
                    int dx = Math.Sign(curr.x - prev.x);
                    int dy = Math.Sign(curr.y - prev.y);

                    while(prev != curr)
                    {
                        prev = (prev.x + dx, prev.y + dy);
                        filled.Add(prev);
                    }

                    prev = curr;
                }
            }
        }

        private bool IsFilled((int x, int y) coord) => coord.y == lowest + 2 || filled.Contains(coord);

        private int Sandfall(bool part1)
        {
            FillRocks();
            lowest = filled.Select(coord => coord.y).Max();

            int i;
            for (i = 0; !filled.Contains((500, 0)); i++)
            {
                (int x, int y) sand = (500, 0);
                while (true)
                {
                    // Descend into the abyss
                    if (part1 && sand.y > lowest) return i;
                    // Try going down
                    sand.y++;
                    if (IsFilled(sand))
                    {
                        // Try going down left
                        sand.x--;
                        if (IsFilled(sand))
                        {
                            // Try going down right
                            sand.x += 2;
                            if (IsFilled(sand))
                            {
                                filled.Add((sand.x - 1, sand.y - 1));
                                break;
                            }
                        }
                    }
                }
            }

            return i;
        }

        public int Part1() => Sandfall(true);

        public int Part2() => Sandfall(false);
    }
}