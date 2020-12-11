using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

List<int> Input = new List<int> { 0 };
Input.AddRange(File.ReadAllLines("InputMedium.txt").Select(x => int.Parse(x)).OrderBy(x => x));
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


// How many valid adapters can be reached from each node?
int[] routesForward = new int[Input.Count - 1];

for (int i = 0; i < Input.Count - 1; i++)
{
    routesForward[i] = 1;
    int plusOneDiff = Input[i + 1] - Input[i];
    int plusTwoDiff = i < Input.Count - 2 ? Input[i + 2] - Input[i] : int.MaxValue;
    int plusThreeDiff = i < Input.Count - 3 ? Input[i + 3] - Input[i] : int.MaxValue;

    if (plusOneDiff == 3)
    {
        // Only one route ahead because the difference is the maximum of 3
        continue;
    }
    if (plusTwoDiff <= 3)
    {
        routesForward[i] = 2;
    }
    if (plusThreeDiff == 3)
    {
        // Implies previous two differences were 1 also
        routesForward[i] = 3;
    }
}

long wf = 0;
WaysForward(0, wf);


long totalRoutes = 1;
for (int i = routesForward.Length - 1; i >= 0; i--)
{
    totalRoutes *= routesForward[i];
}

void WaysForward(int i, long currentCount)
{
    
}

Debugger.Break();