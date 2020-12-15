using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

var Input = new List<int> { 0 };
Input.AddRange(File.ReadAllLines("Input.txt").Select(x => int.Parse(x)).OrderBy(x => x));
Input.Add(Input[^1] + 3);

int ones = 1;
int threes = 1;

for (int i = 0; i < Input.Count - 1; i++)
{
    switch (Input[i + 1] - Input[i])
    {
        case 1:
            ones++;
            break;
        case 3:
            threes++;
            break;
    }
}

Console.WriteLine($"Q1: {ones * threes}");

Dictionary<int, long> pathLength = new();
List<int> visited = new();
var work = new Queue<int>();
work.Enqueue(0);
pathLength[0] = 1;

do
{
    if (!work.TryDequeue(out int current))
    {
        break;
    }
    foreach (int adapter in Input.Where(x => x > current && x <= current + 3))
    {
        if (pathLength.ContainsKey(adapter))
        {
            pathLength[adapter] += pathLength[current];
        }
        else 
        {
            pathLength[adapter] = pathLength[current];
        }

        // Add this to be processed if we haven't dealt with it previously
        if (!visited.Contains(adapter))
        {
            work.Enqueue(adapter);
            visited.Add(adapter);
        }
    }
} while(true);

Console.WriteLine($"Q2: {pathLength[Input[^1]]}");