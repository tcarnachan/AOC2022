using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day9
    {
        string inp;

        public Day9()
        {
            string[][] tmp = InputHandler.GetInput(9).SplitTwice('\n', ' ');
            inp = string.Concat(tmp.Map(s => new string(s[0][0], int.Parse(s[1]))));
        }

        private (int, int) MoveHead((int x, int y) head, char dir)
            => dir switch
            {
                'R' => (head.x + 1, head.y),
                'L' => (head.x - 1, head.y),
                'U' => (head.x, head.y - 1),
                _ => (head.x, head.y + 1)
            };

        private (int, int) MoveTail((int x, int y) head, (int x, int y) tail)
            => Math.Abs(head.x - tail.x) > 1 || Math.Abs(head.y - tail.y) > 1
                ? (tail.x + head.x.CompareTo(tail.x), tail.y + head.y.CompareTo(tail.y))
                : tail;

        private int SimulateRope(int ropeSize)
        {
            (int, int)[] rope = new (int, int)[ropeSize];
            HashSet<(int, int)> visited = new HashSet<(int, int)>();

            foreach (char dir in inp)
            {
                rope[0] = MoveHead(rope[0], dir);
                for (int j = 1; j < rope.Length; j++)
                    rope[j] = MoveTail(rope[j - 1], rope[j]);
                visited.Add(rope.Last());
            }

            return visited.Count();
        }

        public int Part1() => SimulateRope(2);

        public int Part2() => SimulateRope(10);
    }
}