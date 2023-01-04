using System;
namespace AOC2022
{
    public class Day22
    {
        enum Facing { Right, Down, Left, Up }
        (int, int)[] dirs = { (0, 1), (1, 0), (0, -1), (-1, 0) };

        string[] inp;
        string[] instrs;

        public Day22()
        {
            string[][] file = InputHandler.GetInput(22).SplitTwice("\n\n", "\n");

            inp = file[0];
            // Pad so all lines are same length
            int len = inp.Select(l => l.Length).Max();
            for (int i = 0; i < inp.Length; i++)
                inp[i] += new string(' ', len - inp[i].Length);

            instrs = file[1][0].Replace("R", " R ").Replace("L", " L ").Split();
        }

        public int Part1()
        {
            (int r, int c, Facing dir) curr = (0, inp[0].IndexOf('.'), Facing.Right);

            foreach (string instr in instrs)
            {
                int n;
                if (int.TryParse(instr, out n))
                {
                    (int dr, int dc) = dirs[(int)curr.dir];
                    for (int i = 0; i < n; i++)
                    {
                        (int nr, int nc) = (curr.r + dr, curr.c + dc);
                        // Wrap to bottom
                        if (nr < 0 || (dr == -1 && inp[nr][nc] == ' '))
                            for (nr = inp.Length - 1; inp[nr][nc] == ' '; nr--) ;
                        // Wrap to top
                        else if (nr >= inp.Length || (dr == 1 && inp[nr][nc] == ' '))
                            for (nr = 0; inp[nr][nc] == ' '; nr++) ;
                        // Wrap to right
                        if (nc < 0 || (dc == -1 && inp[nr][nc] == ' '))
                            for (nc = inp[nr].Length - 1; inp[nr][nc] == ' '; nc--) ;
                        // Wrap to left
                        else if (nc >= inp[nr].Length || (dc == 1 && inp[nr][nc] == ' '))
                            for (nc = 0; inp[nr][nc] == ' '; nc++) ;

                        // Check if we can move there
                        if (inp[nr][nc] == '.') curr = (nr, nc, curr.dir);
                        else break;
                    }
                }
                else
                {
                    if (instr == "R") curr.dir = curr.dir.Next();
                    else curr.dir = curr.dir.Prev();
                }
            }

            return 1000 * (curr.r + 1) + 4 * (curr.c + 1) + (int)curr.dir;
        }

        enum Face { U, R, F, D, L, B };

        public int Part2()
        {
            int faceSize = 50;

            (int, int)[] faceCoords = new (int, int)[]
            {
                // U           R           F
                (  0,  50), (  0, 100), ( 50,  50),
                // D           L           B
				(100,  50), (100,   0), (150,   0)
            };

            char[][][] cube = new char[6][][];
            for (int i = 0; i < 6; i++)
            {
                char[][] face = new char[faceSize][];

                (int sr, int sc) = faceCoords[i];

                for (int r = 0; r < faceSize; r++)
                {
                    char[] row = new char[faceSize];
                    for (int c = 0; c < faceSize; c++)
                        row[c] = inp[sr + r][sc + c];
                    face[r] = row;
                }

                cube[i] = face;
            }

            (int r, int c, Facing dir, Face f) curr = (0, 0, Facing.Right, Face.U);

            foreach (string instr in instrs)
            {
                int n;
                if (int.TryParse(instr, out n))
                {
                    for (int i = 0; i < n; i++)
                    {
                        var prev = curr;

                        switch (curr.dir)
                        {
                            case Facing.Right:
                                curr.c++;
                                if (curr.c == faceSize)
                                {
                                    curr = curr.f switch
                                    {
                                        Face.U => (curr.r, 0, Facing.Right, Face.R),
                                        Face.R => (faceSize - curr.r - 1, faceSize - 1, Facing.Left, Face.D),
                                        Face.F => (faceSize - 1, curr.r, Facing.Up, Face.R),
                                        Face.D => (faceSize - curr.r - 1, faceSize - 1, Facing.Left, Face.R),
                                        Face.L => (curr.r, 0, Facing.Right, Face.D),
                                        Face.B => (faceSize - 1, curr.r, Facing.Up, Face.D)
                                    };
                                }
                                break;
                            case Facing.Left:
                                curr.c--;
                                if (curr.c == -1)
                                {
                                    curr = curr.f switch
                                    {
                                        Face.U => (faceSize - curr.r - 1, 0, Facing.Right, Face.L),
                                        Face.R => (curr.r, faceSize - 1, Facing.Left, Face.U),
                                        Face.F => (0, curr.r, Facing.Down, Face.L),
                                        Face.D => (curr.r, faceSize - 1, Facing.Left, Face.L),
                                        Face.L => (faceSize - curr.r - 1, 0, Facing.Right, Face.U),
                                        Face.B => (0, curr.r, Facing.Down, Face.U)
                                    };
                                }
                                break;
                            case Facing.Down:
                                curr.r++;
                                if (curr.r == faceSize)
                                {
                                    curr = curr.f switch
                                    {
                                        Face.U => (0, curr.c, Facing.Down, Face.F),
                                        Face.R => (curr.c, faceSize - 1, Facing.Left, Face.F),
                                        Face.F => (0, curr.c, Facing.Down, Face.D),
                                        Face.D => (curr.c, faceSize - 1, Facing.Left, Face.B),
                                        Face.L => (0, curr.c, Facing.Down, Face.B),
                                        Face.B => (0, curr.c, Facing.Down, Face.R)
                                    };
                                }
                                break;
                            case Facing.Up:
                                curr.r--;
                                if (curr.r == -1)
                                {
                                    curr = curr.f switch
                                    {
                                        Face.U => (curr.c, 0, Facing.Right, Face.B),
                                        Face.R => (faceSize - 1, curr.c, Facing.Up, Face.B),
                                        Face.F => (faceSize - 1, curr.c, Facing.Up, Face.U),
                                        Face.D => (faceSize - 1, curr.c, Facing.Up, Face.F),
                                        Face.L => (curr.c, 0, Facing.Right, Face.F),
                                        Face.B => (faceSize - 1, curr.c, Facing.Up, Face.L)
                                    };
                                }
                                break;
                        }

                        if (cube[(int)curr.f][curr.r][curr.c] == '#')
                        {
                            curr = prev;
                            break;
                        }
                    }
                }
                else
                {
                    if (instr == "R") curr.dir = curr.dir.Next();
                    else curr.dir = curr.dir.Prev();
                }
            }

            (int dr, int dc) = faceCoords[(int)curr.f];
            return 1000 * (curr.r + dr + 1) + 4 * (curr.c + dc + 1) + (int)curr.dir;
        }
    }
}