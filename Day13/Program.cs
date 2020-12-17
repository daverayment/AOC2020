using System;
using System.IO;
using System.Linq;

string[] Input = File.ReadAllLines("Input.txt");
int timestamp = int.Parse(Input[0]);
var buses = Input[1].Split(',').Where(x => x != "x").Select(x => int.Parse(x)).ToList();

int lowestRem = int.MaxValue;
int lowestBus = 0;

foreach (int bus in buses) 
{
    int rem = timestamp % bus;
    if (rem < lowestRem)
    {
        lowestBus = bus;
        lowestRem = rem;
    }
}

Console.WriteLine($"Q1: {lowestBus * lowestRem}");

