using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace AOC2022
{
    /*
     * Class to handle getting the input and some
     * useful functions for parsing it
     */
    public static class InputHandler
    {
        public static string GetInput(int day)
        {
            string filename = $"day{day}.txt", input;
            if (!File.Exists(filename))
            {
                string sessionId = File.ReadAllText("session.txt");
                WebClient client = new WebClient();
                client.Headers.Add(HttpRequestHeader.Cookie, $"session={sessionId}");
                input = client.DownloadString($"https://adventofcode.com/2022/day/{day}/input").TrimEnd('\n');
                File.WriteAllText(filename, input);
            }
            else
            {
                input = File.ReadAllText(filename);
            }
            return input;
        }

        public static string[] Split(this string s, string delim, bool removeEmpty = false)
        {
            return s.Split(new string[] { delim },
                removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        public static string[][] SplitTwice(this string s, char delim1 = '\n', char delim2 = '\n', bool removeEmpty = true)
        {
            return removeEmpty ?
                s.SplitNonEmpty(delim1).Map(str => str.SplitNonEmpty(delim2)) :
                s.Split(delim1).Map(str => str.Split(delim2));
        }

        public static string[][] SplitTwice(this string s, string delim1, string delim2, bool removeEmpty = false)
            => s.Split(delim1, removeEmpty).Map(str => str.Split(delim2, removeEmpty));

        public static int[][] SplitTwiceInts(this string s, char delim1, char delim2)
            => s.SplitNonEmpty(delim1).Map(str => str.SplitInts(delim2));

        public static int[][] SplitTwiceInts(this string s, string delim1, string delim2)
            => s.Split(delim1).Map(str => str.SplitInts(delim2));

        public static int[] SplitInts(this string s, char delim = '\n')
            => s.SplitNonEmpty(delim).Map(int.Parse);

        public static int[] SplitInts(this string s, string delim = "\n")
            => s.Split(delim, StringSplitOptions.RemoveEmptyEntries).Map(int.Parse);

        // Split a string and remove empty entries
        public static string[] SplitNonEmpty(this string s, char delim)
            => s.Split(new char[] { delim }, StringSplitOptions.RemoveEmptyEntries);

        // Get all integers from a string
        public static int[] GetInts(this string s)
            => Regex.Split(s, @"\D+").Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToArray();
    }
}
