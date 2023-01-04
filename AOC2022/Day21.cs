using System;
namespace AOC2022
{
    public class Day21
    {
        Dictionary<string, string> inp;

        public Day21()
        {
            string[][] file = InputHandler.GetInput(21).SplitTwice("\n", ": ");
            inp = file.ToDictionary(line => line[0], line => line[1]);
        }

        public long Part1() => (long)GetExpr("root").value;

        private Expression GetExpr(string monkey)
        {
            if (!inp.ContainsKey(monkey))
                return new Expression(monkey);
            string[] op = inp[monkey].Split();
            if (op.Length == 1)
                return new Expression(long.Parse(op[0]));

            Expression left = GetExpr(op[0]);
            Expression right = GetExpr(op[2]);

            if (left.value != null && right.value != null)
            {
                long lhs = (long)left.value;
                long rhs = (long)right.value;
                return new Expression(op[1] switch
                {
                    "+" => lhs + rhs,
                    "-" => lhs - rhs,
                    "*" => lhs * rhs,
                     _  => lhs / rhs
                });
            }

            return new Expression(left, right, op[1]);
        }

        public long Part2()
        {
            string[] root = inp["root"].Split();

            inp.Remove("humn");

            Expression expr = GetExpr(root[0]);
            long target = (long)GetExpr(root[2]).value;
            Console.WriteLine(target);

            for(int i = 0; ; i++)
            {
                if (expr.lhs == null || expr.rhs == null || expr.op == null)
                    break;

                Expression lhs = expr.lhs, rhs = expr.rhs;

                if(lhs.value != null)
                {
                    if (expr.op == "+") target -= (long)lhs.value;
                    if (expr.op == "-") target = (long)lhs.value - target;
                    if (expr.op == "*") target /= (long)lhs.value;
                    if (expr.op == "/") target = (long)lhs.value / target;
                    expr = rhs;
                }
                else if (rhs.value != null)
                {
                    if (expr.op == "+") target -= (long)rhs.value;
                    if (expr.op == "-") target += (long)rhs.value;
                    if (expr.op == "*") target /= (long)rhs.value;
                    if (expr.op == "/") target *= (long)rhs.value;
                    expr = lhs;
                }
            }

            return target;
        }

        class Expression
        {
            public Expression? lhs = null;
            public Expression? rhs = null;
            public string? op = null;

            public long? value = null;
            public string? str = null;

            public Expression(Expression lhs, Expression rhs, string op)
            {
                this.lhs = lhs;
                this.rhs = rhs;
                this.op = op;
            }

            public Expression(long value)
            {
                this.value = value;
            }

            public Expression(string str)
            {
                this.str = str;
            }

            public override string ToString()
            {
                if (value != null)
                    return "" + value;
                if (str != null)
                    return str;
                return $"({lhs} {op} {rhs})";
            }
        }
    }
}

