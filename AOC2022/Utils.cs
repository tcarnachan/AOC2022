using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    /* Useful functions for aoc. */
    public static class Utils
    {
        // Includes start, excludes end
        public static IEnumerable<int> Range(int end) => Enumerable.Range(0, end);
        public static IEnumerable<int> Range(int start, int end) => Enumerable.Range(start, end - start);

        // Saves having to .ToArray or .ToList everywhere
        public static T[] Map<S, T>(this S[] s, Func<S, T> f) => s.Select(f).ToArray();
        public static List<T> Map<S, T>(this List<S> s, Func<S, T> f) => s.Select(f).ToList();

        // Get next value of enum
        public static T Next<T>(this T src) where T : Enum
        {
            T[] vals = (T[])Enum.GetValues(typeof(T));
            int ix = Array.IndexOf(vals, src) + 1;
            return ix == vals.Length ? vals[0] : vals[ix];
        }

        // Get previous value of enum
        public static T Prev<T>(this T src) where T : Enum
        {
            T[] vals = (T[])Enum.GetValues(typeof(T));
            int ix = Array.IndexOf(vals, src) - 1;
            return ix == -1 ? vals.Last() : vals[ix];
        }

        // Needed for handling when n < 0
        public static int Mod(long n, int m) => (int)(((n % m) + m) % m);

        // Intersection of IEnumerable<T>s
        public static IEnumerable<T> Intersect<T>(IEnumerable<IEnumerable<T>> sets)
            => sets.Aggregate((s1, s2) => s1.Intersect(s2));

        // Rotate a jagged array
        public static T[][] Rot<T>(T[][] arr)
        {
            T[][] res = new T[arr[0].Length][];

            for (int r = 0; r < res.Length; r++)
            {
                res[r] = new T[arr.Length];
                for (int c = 0; c < arr.Length; c++)
                    res[r][c] = arr[arr.Length - c - 1][r];
            }

            return res;
        }
    }
}
