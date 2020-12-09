using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

int maxSeatId = 0;
var assignedSeatIds = new HashSet<int>();

foreach (string pass in File.ReadAllLines("Input.txt"))
{
    int seatId = DecodeSeatId(pass);
    assignedSeatIds.Add(seatId);
    maxSeatId = Math.Max(maxSeatId, seatId);
}
Console.WriteLine($"Q1: {maxSeatId}");

for (int i = 0; i < maxSeatId; i++)
{
    if (!assignedSeatIds.Contains(i) &&
        assignedSeatIds.Contains(i + 8) && 
        assignedSeatIds.Contains(i - 8))
    {
        Console.WriteLine($"Q2: {i}");
        break;
    }
}

static int DecodeSeatId(string pass)
{
    int range = 128;
    int row = 128;
    int col = 8;

    // Seat row
    foreach (char ch in pass.Take(7))
    {
        if (ch == 'F')
        {
            row -= range / 2;
        }
        range /= 2;
    }

    // Seat column
    range = 8;
    foreach (char ch in pass.Skip(7).Take(3))
    {
        if (ch == 'L')
        {
            col -= range / 2;
        }
        range /= 2;
    }

    // NB: zero-based, so subtract 1 from each
    return (row - 1) * 8 + (col - 1);
}