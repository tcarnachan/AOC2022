using System;
namespace AOC2022
{
    public class Day12
    {
        char[] inp;
        static int rows, cols;
        int start, end;

        public Day12()
        {
            string[] file = InputHandler.GetInput(12).Split();
            inp = string.Concat(file).ToCharArray();
            (rows, cols) = (file.Length, file[0].Length);

            inp[start = Array.IndexOf(inp, 'S')] = 'a';
            inp[end = Array.IndexOf(inp, 'E')] = 'z';
        }

        private int BFS(IEnumerable<int> startList)
        {
            HashSet<int> visited = new HashSet<int>(startList);
            Queue<(int, int)> queue = new Queue<(int, int)>();
            foreach (int ix in startList) queue.Enqueue((ix, 0));

            while (queue.Count() > 0)
            {
                (int ix, int depth) = queue.Dequeue();
                char elev = inp[ix];
                if (ix == end) return depth;

                foreach (int delta in new int[] { -1, 1, -cols, cols })
                {
                    int nIx = ix + delta;
                    if (nIx >= 0 && nIx < inp.Length && inp[nIx] - elev < 2 &&
                        (nIx % cols - ix % cols == 0 || nIx / cols - ix / cols == 0) && visited.Add(nIx))
                    {
                        queue.Enqueue((nIx, depth + 1));
                    }
                }
            }

            return -1;
        }

        public int Part1() => BFS(new int[] { start });

        public int Part2() => BFS(Utils.Range(inp.Length).Where(ix => inp[ix] == 'a'));
    }
}