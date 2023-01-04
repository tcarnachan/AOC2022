using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day10
    {
        string[][] inp;

        public Day10()
        {
            inp = InputHandler.GetInput(10).SplitTwice('\n', ' ');
        }

        public int Part1()
        {
            int x = 1, sum = 0, cycle = 1;
            foreach (string[] line in inp)
            {
                cycle++;
                if ((cycle - 20) % 40 == 0)
                    sum += cycle * x;
                if (line[0] == "addx")
                {
                    cycle++;
                    x += int.Parse(line[1]);
                    if ((cycle - 20) % 40 == 0)
                        sum += cycle * x;
                }
            }
            return sum;
        }

        private (int, int) GetPx(int cycle, int w) => ((cycle - 1) / w, (cycle - 1) % w);

        public string Part2()
        {
            int cycle = 0, x = 1;
            int width = 40, height = 6;
            char[][] display = new char[height][];
            for (int i = 0; i < height; i++)
                display[i] = new string(' ', width).ToCharArray();

            foreach (string[] line in inp)
            {
                cycle++;
                (int r, int c) = GetPx(cycle, width);
                if (Math.Abs(c - x) <= 1) display[r][c] = '#';

                if (line[0] == "addx")
                {
                    cycle++;
                    (r, c) = GetPx(cycle, width);
                    if (Math.Abs(c - x) <= 1) display[r][c] = '#';
                    x += int.Parse(line[1]);
                }
            }
            Display(display);
            return "\n" + string.Join("\n", display.Map(string.Concat));
        }

        private void Display(char[][] display)
        {
            foreach (char[] cs in display)
            {
                foreach(char c in cs)
                {
                    Console.BackgroundColor = c == '#' ? ConsoleColor.White : ConsoleColor.Black;
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }
}