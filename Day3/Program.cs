using System;
using System.IO;

string[] Input = File.ReadAllLines("Input.txt");

Console.WriteLine("Q1: " + TreeCheck(Input, incY: 1, incX: 3));

long treeSum = 1;
foreach (var check in new[] {(1, 1), (1, 3), (1, 5), (1, 7), (2, 1)})
{
    treeSum *= TreeCheck(Input, check.Item1, check.Item2);
}
Console.WriteLine("Q2: " + treeSum);

static int TreeCheck(string[] input, int incY, int incX)
{
    int treesHit = 0;
    int x = 0;
    int y = incY;   // NB: do not count first row

    do
    {
        x = (x + incX) % input[0].Length;
        if (input[y][x] == '#')
        {
            treesHit++;
        }
        y += incY;
    } while (y < input.Length);

    return treesHit;
}