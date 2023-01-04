using System;
namespace AOC2022
{
	public class Day20
	{
		long[] inp;

		public Day20()
		{
			inp = InputHandler.GetInput(20).Split().Map(long.Parse);
        }

		private long Decrypt(int iters)
		{
            int len = inp.Length;
            List<int> indexes = Utils.Range(len).ToList();

            for (int _ = 0; _ < iters; _++)
            {
                for (int i = 0; i < len; i++)
                {
                    int ix = indexes.IndexOf(i);
                    indexes.Remove(i);
                    int nix = Utils.Mod(ix + inp[i], len - 1);
                    if (nix == len - 1) indexes.Add(i);
                    else indexes.Insert(nix, i);
                }
            }

            int zero = indexes.IndexOf(Array.IndexOf(inp, 0));
            long sum = 0;
            for (int x = 1000; x <= 3000; x += 1000)
                sum += inp[indexes[(zero + x) % len]];

            return sum;
        }

        public long Part1() => Decrypt(1);

		public long Part2()
		{
            for (int i = 0; i < inp.Length; i++)
                inp[i] *= 811589153;
            return Decrypt(10);
        }
	}
}

