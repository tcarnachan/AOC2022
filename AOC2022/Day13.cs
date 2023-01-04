using System;
namespace AOC2022
{
    public class Day13
    {
        List<Item> packets = new List<Item>();

        public Day13()
        {
            string[] file = InputHandler.GetInput(13).SplitNonEmpty('\n');
            packets = ParseInput(file).Map(s => buildPacket(s, 1).Item1);
        }

        private List<List<string>> ParseInput(string[] file)
        {
            List<List<string>> input = new List<List<string>>();
            foreach (string line in file)
            {
                List<string> inpLine = new List<string>();
                List<char> curr = new List<char>();
                foreach (char c in line)
                {
                    if (",[]".Contains(c))
                    {
                        if (curr.Count() != 0)
                        {
                            inpLine.Add(string.Concat(curr));
                            curr.Clear();
                        }
                        if (c != ',') inpLine.Add("" + c);
                    }
                    else curr.Add(c);
                }
                input.Add(inpLine);
            }

            return input;
        }

        private (Item, int) buildPacket(List<string> s, int ix)
        {
            List<Item> items = new List<Item>();
            while (s[ix] != "]")
            {
                if (s[ix] == "[")
                {
                    (Item item, ix) = buildPacket(s, ix + 1);
                    items.Add(item);
                }
                else
                    items.Add(new Item(int.Parse(s[ix++])));
            }
            return (new Item(items), ix + 1);
        }

        public int Part1()
        {
            int sum = 0;
            for (int i = 0; i < packets.Count(); i += 2)
            {
                if (packets[i].CompareTo(packets[i + 1]) < 1)
                    sum += i / 2 + 1;
            }
            return sum;
        }

        public int Part2()
        {
            List<List<string>> dividers = ParseInput(new string[] { "[[2]]", "[[6]]" });
            List<Item> divPackets = dividers.Map(s => buildPacket(s, 1).Item1);
            packets.AddRange(divPackets);

            packets.Sort();

            return (packets.IndexOf(divPackets[0]) + 1) * (packets.IndexOf(divPackets[1]) + 1);
        }

        private class Item : IComparable<Item>
        {
            public int value = -1;
            public List<Item> items = new List<Item>();

            public Item(int value) => this.value = value;
            public Item(List<Item> items) => this.items = items;

            public int CompareTo(Item? other)
            {
                if (other == null) return 1;

                if (value != -1 && other.value != -1)
                    return value.CompareTo(other.value);
                else if (value == -1 && other.value == -1)
                {
                    int comp = 0, ix;
                    int maxIx = Math.Min(items.Count(), other.items.Count());
                    for (ix = 0; ix < maxIx && comp == 0; ix++)
                        comp = items[ix].CompareTo(other.items[ix]);
                    if (comp == 0)
                        return items.Count().CompareTo(other.items.Count());
                    return comp;
                }
                else if (value != -1)
                    return new Item(new List<Item> { new Item(value) }).CompareTo(other);
                else
                    return CompareTo(new Item(new List<Item> { new Item(other.value) }));
            }
        }
    }
}