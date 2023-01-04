using System;

namespace AOC2022
{
	public class Day24
	{
		char[][] inp;
		List<(int, int, char)> blizzards = new List<(int, int, char)>();
		(int, int)[] neighbours = new (int, int)[]
			{ (-1, 0), (1, 0), (0, -1), (0, 1) };

		(int, int) start, end;

		public Day24()
		{
			inp = InputHandler.GetInput(24).Split().Map(s => s.ToCharArray());
			//inp = "#.######\n#>>.<^<#\n#.<..<<#\n#>v.><>#\n#<^v^^>#\n######.#".Split().Map(s => s.ToCharArray());

            for (int r = 0; r < inp.Length; r++)
			{
				char[] row = inp[r];
				for(int c = 0; c < row.Length; c++)
				{
					if ("^v<>".Contains(row[c]))
						blizzards.Add((r, c, row[c]));
				}
			}

			start = (0, Array.IndexOf(inp[0], '.'));
            end = (inp.Length - 1, Array.IndexOf(inp.Last(), '.'));
        }

		private int BFS((int, int) start, (int, int) end)
		{
            HashSet<(int, int)> toVisit = new HashSet<(int, int)>();

            toVisit.Add(start);

            for (int min = 0; ; min++)
            {
                // Update blizzards
                HashSet<(int, int)> blizardPos = new HashSet<(int, int)>();
                for (int i = 0; i < blizzards.Count; i++)
                {
                    (int r, int c, char dir) = blizzards[i];
                    (r, c) = dir switch
                    {
                        '^' => (r - 1, c),
                        'v' => (r + 1, c),
                        '<' => (r, c - 1),
                        _ => (r, c + 1)
                    };

                    if (r == 0) r = inp.Length - 2;
                    if (r == inp.Length - 1) r = 1;
                    if (c == 0) c = inp[0].Length - 2;
                    if (c == inp[0].Length - 1) c = 1;

                    blizzards[i] = (r, c, dir);
                    blizardPos.Add((r, c));
                }

                // Try movements
                HashSet<(int, int)> next = new HashSet<(int, int)>();

                foreach ((int r, int c) curr in toVisit)
                {
                    if (!blizardPos.Contains(curr))
                        next.Add(curr);
                    foreach ((int r, int c) dir in neighbours)
                    {
                        (int nr, int nc) = (curr.r + dir.r, curr.c + dir.c);
                        if ((nr, nc) == end) return min + 1;

                        if (nr > 0 && nr < inp.Length - 1 && nc > 0 &&
                            nc < inp[nr].Length - 1 && !blizardPos.Contains((nr, nc)))
                            next.Add((nr, nc));
                    }
                }

                toVisit = next;
            }
        }

        public int Part1() => BFS(start, end);

        public int Part2() => 249 + BFS(end, start) + BFS(start, end);

    }
}

