using System;
using System.IO;

long total = 0;
long totalQ2 = 0;
foreach (string line in File.ReadAllLines("Input.txt"))
{        
    total += new Expression(line).Evaluate().result;

    // Alter precedence for Q2 (see https://en.wikipedia.org/wiki/Operator-precedence_parser)
    string lineQ2 = "(" + line.Replace("(", "((").Replace(")", "))")
        .Replace("*", ") * (") + ")";
    totalQ2 += new Expression(lineQ2).Evaluate().result;
}

Console.WriteLine($"Q1: {total}");
Console.WriteLine($"Q2: {totalQ2}");

public enum Op
{
    NoOp,
    Multiply,
    Add,
    Done
}

public class Expression
{
    public Op Op { get; set; } = Op.NoOp;
    int Pos { get; }
    long RunningTotal { get; set; }

    public Expression(string line, int pos = 0)
    {
        Pos = pos;
        while (Pos < line.Length && Op != Op.Done)
        {
            char ch = line[Pos];
            switch (ch)
            {
                case '(':
                    (long num, int newPos) = new Expression(line, Pos + 1).Evaluate();
                    // This is important, as Evaluate() returns newPos after 
                    // the closing bracket and Pos will be incremented again 
                    // below, too
                    Pos = newPos - 1;
                    UpdateRunningTotal(num);
                    break;

                case ')':
                    Op = Op.Done;
                    break;

                case '*':
                    Op = Op.Multiply;
                    break;

                case '+':
                    Op = Op.Add;
                    break;

                default:
                    if (char.IsDigit(ch))
                    {
                        // NB: all numbers are single-digit
                        num = int.Parse(ch.ToString());
                        UpdateRunningTotal(num);
                    }
                    break;
            }
            Pos++;
        }
    }

    public (long result, int newPos) Evaluate()
    {
        return (RunningTotal, Pos);
    }

    void UpdateRunningTotal(long num)
    {
        switch (Op)
        {
            case Op.Multiply:
                this.RunningTotal *= num;
                break;

            case Op.Add:
                this.RunningTotal += num;
                break;
                
            case Op.NoOp:
                this.RunningTotal = num;
                break;
        }
    }
}