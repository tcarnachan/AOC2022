using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day5
    {
        string[] instrs;
        Stack<char>[] crates;

        public Day5()
        {
            string[][] tmp = InputHandler.GetInput(5).SplitTwice("\n\n", "\n");
            instrs = tmp[1];

            string[] stacks = tmp[0];
            int len = (stacks.Last().Length + 1) / 4;
            crates = new Stack<char>[len];
            for(int col = 0; col < len; col++)
            {
                crates[col] = new Stack<char>();
                int ix = 4 * col + 1;
                for(int row = stacks.Length - 1; row >= 0; row--)
                {
                    if (stacks[row][ix] == ' ') break;
                    crates[col].Push(stacks[row][ix]);
                }
            }
        }

        private Stack<char>[] CopyCrates()
        {
            Stack<char>[] cpy = new Stack<char>[crates.Length];
            for (int i = 0; i < cpy.Length; i++)
                cpy[i] = new Stack<char>(crates[i].Reverse());
            return cpy;
        }

        public string Part1()
        {
            Stack<char>[] crates = CopyCrates();
            foreach (string s in instrs)
            {
                string[] tmp = s.Split();
                int start = int.Parse(tmp[3]) - 1, end = int.Parse(tmp[5]) - 1;
                for (int _ = 0; _ < int.Parse(tmp[1]); _++)
                    crates[end].Push(crates[start].Pop());
            }
            return string.Concat(crates.Map(s => s.Pop()));
        }

        public string Part2()
        {
            Stack<char>[] crates = CopyCrates();
            Stack<char> tmpStack = new Stack<char>();
            foreach (string s in instrs)
            {
                string[] tmp = s.Split();
                int start = int.Parse(tmp[3]) - 1, end = int.Parse(tmp[5]) - 1;

                for (int _ = 0; _ < int.Parse(tmp[1]); _++)
                    tmpStack.Push(crates[start].Pop());

                while (tmpStack.Count > 0)
                    crates[end].Push(tmpStack.Pop());
            }
            return string.Concat(crates.Map(s => s.Pop()));
        }
    }
}
