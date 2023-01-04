using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    class Directory
    {
        public Directory parent;

        public Dictionary<string, Directory> children = new Dictionary<string, Directory>();
        public int fileSize = 0;

        public Directory(Directory parent) => this.parent = parent;

        public int GetSize() => children.Values.Select(d => d.GetSize()).Sum() + fileSize;
    }

    public class Day7
    {
        List<Directory> dirs = new List<Directory>();
        Directory root = new Directory(null);

        public Day7()
        {
            string[][] input = InputHandler.GetInput(7).SplitTwice("$ ", "\n", true);
            Directory curr = root;

            foreach (string[] split in input)
            {
                string[] cmd = split[0].Split();

                switch (cmd[0])
                {
                    case "cd":
                        curr = cmd[1] switch
                        {
                            "/" => root,
                            ".." => curr.parent,
                            _ => curr.children[cmd[1]]
                        };
                        break;
                    case "ls":
                        foreach (string child in split.Skip(1))
                        {
                            string[] file = child.Split(' ');
                            if (file[0] == "dir")
                            {
                                Directory childDir = new Directory(curr);
                                curr.children.Add(file[1], childDir);
                                dirs.Add(childDir);
                            }
                            else curr.fileSize += int.Parse(file[0]);
                        }
                        break;
                }
            }
        }

        public int Part1() => dirs.Select(d => d.GetSize()).Where(i => i <= 100000).Sum();

        public int Part2()
        {
            int spaceRequired = root.GetSize() - 40000000;
            return dirs.Select(d => d.GetSize()).Where(i => i >= spaceRequired).Min();
        }
    }
}