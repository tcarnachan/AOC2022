using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AOC2022
{
    public class Day11
    {
        string[][] inp;
        Monkey[] monkeys;

        public Day11()
        {
            inp = InputHandler.GetInput(11).SplitTwice("\n\n", "\n");
            SetMonkeys();
        }

        private void SetMonkeys()
        {
            monkeys = new Monkey[inp.Length];
            long mod = 1;
            for (int i = 0; i < monkeys.Length; i++)
            {
                monkeys[i] = new Monkey();
                string[] monkeyStr = inp[i];

                // Get items
                monkeys[i].items = new Queue<long>(monkeyStr[1].Split(": ")[1].Split(", ").Select(long.Parse));
                // Get operation
                string[] op = monkeyStr[2].Split(' ');
                monkeys[i].op = (i => {
                    long n = op.Last() switch { "old" => i, _ => long.Parse(op.Last()) };
                    return op[op.Length - 2] switch
                    {
                        "+" => i + n,
                        "-" => i - n,
                        "*" => i * n,
                        _ => i / n
                    };
                });
                // Get test
                int div = int.Parse(monkeyStr[3].Split(' ').Last());
                mod *= div;
                int divTrue = int.Parse(monkeyStr[4].Split(' ').Last());
                int divFalse = int.Parse(monkeyStr[5].Split(' ').Last());
                monkeys[i].test = (i => i % div == 0 ? divTrue : divFalse);
            }
            Monkey.mod = mod;
        }

        private long MonkeyBusiness(int rounds, bool part2)
        {
            SetMonkeys();
            for (int round = 0; round < rounds; round++)
            {
                foreach (Monkey monkey in monkeys)
                    monkey.InspectAll(monkeys, part2);
            }

            monkeys = monkeys.OrderByDescending(m => m.inspections).ToArray();
            return monkeys[0].inspections * monkeys[1].inspections;
        }

        public long Part1() => MonkeyBusiness(20, false);

        public long Part2() => MonkeyBusiness(10000, true);

        private class Monkey
        {
            public Queue<long> items = new Queue<long>();
            public Func<long, long> op = x => x;
            public Func<long, long> test = x => x;

            public long inspections = 0;
            public static long mod;

            public void InspectAll(Monkey[] monkeys, bool part2)
            {
                inspections += items.Count();
                while(items.Count() > 0)
                {
                    long it = items.Dequeue();
                    long worry = part2 ? op(it) % mod : op(it) / 3;
                    monkeys[test(worry)].items.Enqueue(worry);
                }
            }
        }
    }
}