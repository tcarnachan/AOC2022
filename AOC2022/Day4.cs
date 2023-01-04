using System;

namespace AOC2022
{
    public class Day4
    {
        (int, int)[][] input;

        public Day4()
        {
            string[][] tmp = InputHandler.GetInput(4).SplitTwice('\n', ',');
            input = tmp.Map(s => s.Map(p =>
            {
                int[] t = p.SplitInts('-');
                return (t[0], t[1]);
            }));
        }

        public int Part1()
        {
            return input.Count(pair =>
            {
                (int x, int y) elf1 = pair[0], elf2 = pair[1];
                return (elf1.x <= elf2.x && elf1.y >= elf2.y) || (elf1.x >= elf2.x && elf1.y <= elf2.y);
            });
        }

        public int Part2()
        {
            return input.Count(pair =>
            {
                (int x, int y) elf1 = pair[0], elf2 = pair[1];
                return Math.Max(elf1.x, elf2.x) <= Math.Min(elf1.y, elf2.y);
            });
        }
    }
}
