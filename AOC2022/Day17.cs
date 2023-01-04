using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC2022
{
    public class Day17
    {
        string inp;

        public Day17()
        {
            inp = InputHandler.GetInput(17);
        }

        string[][] pieces = new string[][]
        {
            new string[] { "####" },
            new string[]
            {
                ".#.",
                "###",
                ".#."
            },
            new string []
            {
                "..#",
                "..#",
                "###"
            },
            new string []
            {
                "#",
                "#",
                "#",
                "#"
            },
            new string[]
            {
                "##",
                "##"
            }
        };

        private bool Intersects((long x, long y) pos, string[] piece, HashSet<(long, long)> blocked)
        {
            for(int dy = 0; dy < piece.Length; dy++)
            {
                for(int dx = 0; dx < piece[0].Length; dx++)
                {
                    if (piece[piece.Length - dy - 1][dx] == '#' && blocked.Contains((pos.x + dx, pos.y + dy)))
                        return true;
                }
            }
            return false;
        }

        public long Part1()
        {
            HashSet<(long, long)> blocked = new HashSet<(long, long)>();

            long maxHeight = -1;
            int dirIx = 0;

            for(long numPieces = 0; numPieces < 2022; numPieces++)
            {
                // Bottom left corner of the piece
                (long x, long y) piecePos = (2, maxHeight + 4);
                string[] piece = pieces[numPieces % pieces.Length];
                long height = piece.Length, width = piece[0].Length;

                // Move piece
                while (true)
                {
                    // Get pushed
                    char dir = inp[dirIx % inp.Length];
                    dirIx++;
                    if(dir == '<')
                    {
                        long tx = piecePos.x - 1;
                        if (tx >= 0 && !Intersects((tx, piecePos.y), piece, blocked))
                            piecePos.x = tx;
                    }
                    else
                    {
                        long tx = piecePos.x + 1;
                        if (tx + width <= 7 && !Intersects((tx, piecePos.y), piece, blocked))
                            piecePos.x = tx;
                    }

                    // Go down
                    long ty = piecePos.y - 1;
                    if (ty == -1 || Intersects((piecePos.x, ty), piece, blocked))
                    {
                        for (int dy = 0; dy < height; dy++)
                        {
                            for (int dx = 0; dx < width; dx++)
                            {
                                if (piece[height - dy - 1][dx] == '#')
                                {
                                    if (piecePos.y + dy > maxHeight) maxHeight = piecePos.y + dy;
                                    blocked.Add((piecePos.x + dx, piecePos.y + dy));
                                }
                            }
                        }
                        break;
                    }
                    piecePos.y = ty;
                }
            }

            return maxHeight + 1;
        }

        private string GetCurrFormation(HashSet<(long, long)> blocked, long maxHeight, long numRows)
        {
            if (numRows == 0) return "";
            string res = "";
            long start = Math.Max(maxHeight, numRows);
            long end = start - numRows;
            for(long i = start; i >= end; i--)
            {
                for (long j = 0; j < 7; j++)
                    res += blocked.Contains((j, i)) ? '#' : ' ';
                res += '\n';
            }
            return res;
        }

        public long Part2()
        {
            HashSet<(long, long)> blocked = new HashSet<(long, long)>();
            Dictionary<(string, long, long), (long, long)> history = new Dictionary<(string, long, long), (long, long)>();

            long maxHeight = -1;
            int dirIx = 0;

            long targetPieces = 1000000000000, addHeight = 0;
            bool repeated = false;

            for (long numPieces = 0; numPieces <= targetPieces ; numPieces++)
            {
                // Bottom left corner of the piece
                (long x, long y) piecePos = (2, maxHeight + 4);
                string[] piece = pieces[numPieces % pieces.Length];
                long height = piece.Length, width = piece[0].Length;

                // Move piece
                while (true)
                {
                    // Get pushed
                    char dir = inp[dirIx % inp.Length];
                    dirIx++;
                    if (dir == '<')
                    {
                        long tx = piecePos.x - 1;
                        if (tx >= 0 && !Intersects((tx, piecePos.y), piece, blocked))
                            piecePos.x = tx;
                    }
                    else
                    {
                        long tx = piecePos.x + 1;
                        if (tx + width <= 7 && !Intersects((tx, piecePos.y), piece, blocked))
                            piecePos.x = tx;
                    }

                    // Go down
                    long ty = piecePos.y - 1;
                    if (ty == -1 || Intersects((piecePos.x, ty), piece, blocked))
                    {
                        for (int dy = 0; dy < height; dy++)
                        {
                            for (int dx = 0; dx < width; dx++)
                            {
                                if (piece[height - dy - 1][dx] == '#')
                                {
                                    if (piecePos.y + dy > maxHeight) maxHeight = piecePos.y + dy;
                                    blocked.Add((piecePos.x + dx, piecePos.y + dy));
                                }
                            }
                        }
                        break;
                    }
                    piecePos.y = ty;
                }

                // The number 16 here is what I found to be the minimum value that works, I got the initial
                // answer with the completely arbitrary value of 20 here instead. There's probably a better
                // way than choosing random numbers here.
                var key = (GetCurrFormation(blocked, maxHeight, 0), numPieces % pieces.Length, dirIx % inp.Length);
                if(!repeated && history.ContainsKey(key))
                {
                    repeated = true;

                    long range = numPieces - history[key].Item1;
                    long numReps = (targetPieces - numPieces) / range;

                    numPieces += range * numReps;
                    addHeight = (maxHeight - history[key].Item2) * numReps;
                }
                else history[key] = (numPieces, maxHeight);
            }

            return maxHeight + addHeight;
        }
    }
}