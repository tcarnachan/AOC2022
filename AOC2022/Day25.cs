using System;
namespace AOC2022
{
	public class Day25
	{
		string[] inp;

		public Day25()
		{
			inp = InputHandler.GetInput(25).Split();
		}

		public string Part1()
		{
			long target = inp.Map(s =>
			{
				long n = 0;
				foreach(char c in s)
					n = n * 5 + "=-012".IndexOf(c) - 2;
				return n;
            }).Sum();

            string res = "";

            while (target > 0)
            {
                long m = target % 5;
                if (m <= 2)
                {
                    res = m.ToString() + res;
                    target /= 5;
                }
                else
                {
                    res = "-="[4 - (int)m] + res;
                    target = target / 5 + 1;
                }
            }

            return res;
        }

        public string Part2() => "Smoothie!";
	}
}

