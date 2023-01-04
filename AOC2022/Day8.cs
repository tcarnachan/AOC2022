using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day8
    {
        int[][][] rot;
        int maxR, maxC;

        public Day8()
        {
            string[] input = InputHandler.GetInput(8).Split();
            int[][] inp = input.Map(s => s.ToCharArray().Map(c => c - '0'));
            (maxR, maxC) = (inp.Length - 1, inp[0].Length - 1);

            // Rotated versions of inp
            rot = new int[4][][];
            rot[0] = inp;
            for (int i = 1; i < rot.Length; i++)
                rot[i] = Utils.Rot(rot[i - 1]);
        }

        private void AddVisibleFromLeft(int[][] inp, ref HashSet<(int, int)> visible, Func<(int, int), (int, int)> GetIx)
        {
            for (int r = 0; r < inp[0].Length; r++)
            {
                int[] inpRow = inp[r];
                int currMax = -1;
                for (int c = 0; c < inp.Length; c++)
                {
                    if (inpRow[c] > currMax)
                    {
                        currMax = inpRow[c];
                        visible.Add(GetIx((r, c)));
                    }
                }
            }
        }

        public int Part1()
        {
            HashSet<(int, int)> visible = new HashSet<(int, int)>();

            // Get visible from left
            AddVisibleFromLeft(rot[0], ref visible, ix => ix);
            // Get visible from bottom
            AddVisibleFromLeft(rot[1], ref visible, ix => (maxR - ix.Item2, ix.Item1));
            // Get visible from right
            AddVisibleFromLeft(rot[2], ref visible, ix => (maxR - ix.Item1, maxC - ix.Item2));
            // Get visible from top
            AddVisibleFromLeft(rot[3], ref visible, ix => (ix.Item2, maxC - ix.Item1));

            return visible.Count();
        }

        private int ScoreFromTop(int[][] inp, int r, int c)
        {
            for (int dr = 1; r - dr >= 0; dr++)
            {
                if (r - dr == 0 || inp[r - dr][c] >= inp[r][c])
                    return dr;
            }
            return 0;
        }

        private int ScenicScore(int r, int c)
        {
            // Up
            int score = ScoreFromTop(rot[0], r, c);
            // Left
            score *= ScoreFromTop(rot[1], c, maxR - r);
            // Down
            score *= ScoreFromTop(rot[2], maxR - r, maxR - c);
            // Right
            score *= ScoreFromTop(rot[3], maxC - c, r);

            return score;
        }

        public int Part2()
        {
            int max = 0;

            for (int r = 1; r < rot[0].Length - 1; r++)
            {
                for (int c = 1; c < rot[0][r].Length - 1; c++)
                    max = Math.Max(max, ScenicScore(r, c));
            }

            return max;
        }
    }
}