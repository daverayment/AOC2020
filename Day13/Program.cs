using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

string[] Input = File.ReadAllLines("Input.txt");
int timestamp = int.Parse(Input[0]);
var buses = Input[1].Split(',').Where(x => x != "x").Select(x => int.Parse(x)).ToList();

int waitTime = int.MaxValue;
int bestBus = 0;

foreach (int bus in buses) 
{
    int departs = bus - (timestamp % bus);
    if (departs < waitTime)
    {
        waitTime = departs;
        bestBus = bus;
    }
}

Console.WriteLine($"Q1: {waitTime * bestBus}");

var allBuses = Input[1].Split(',').ToArray();
// Hold bus ID and time offset
var q2Buses = new Dictionary<uint, uint>();
for (uint i = 0; i < allBuses.Length; i++)
{
    if (allBuses[i] != "x")
    {
        q2Buses.Add(uint.Parse(allBuses[i]), i);
    }
}

// Check this often for the next bus in the sequence
ulong skip = q2Buses.First().Key;
ulong pos = skip;

foreach (var bus in q2Buses.Skip(1))
{
    while ((pos + bus.Value) % bus.Key != 0)
    {
        pos += skip;
    }
    // The positions can only line up in multiples of the previous keys
    // multipled together.
    skip *= bus.Key;
}

Console.WriteLine($"Q2: " + pos);