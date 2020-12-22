using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

ulong andMask = ~0ul;
ulong orMask = 0;
Dictionary<ulong, ulong> memory = new();

foreach (string line in File.ReadAllLines("Input.txt"))
{
    var parts = line.Split(' ');
    if (parts[0] == "mask")
    {
        andMask = ~0ul;
        orMask = 0;
        for (int i = 0; i < parts[2].Length; i++)
        {
            ulong bitVal = 1ul << (35 - i);
            switch (parts[2][i])
            {
                case '0':
                    // Clear current bit
                    andMask &= ~bitVal;
                    break;
                case '1':
                    // Set bit
                    orMask |= bitVal;
                    andMask |= bitVal;
                    break;
            }
        }
    }
    else
    {
        var match = Regex.Match(line, @"^mem\[(\d+)\] = (\d+)");
        ulong address = ulong.Parse(match.Groups[1].Value);
        ulong operand = ulong.Parse(match.Groups[2].Value);
        if (!memory.ContainsKey(address))
        {
            memory.Add(address, 0);
        }
        operand |= orMask;
        operand &= andMask;
        memory[address] = operand;
    }
}

ulong total = 0ul;
foreach (var kv in memory)
{
    total += kv.Value;
}
Console.WriteLine($"Q1: {total}");

memory.Clear();
List<ulong> floatBits = new();

foreach (string line in File.ReadAllLines("Input.txt"))
{
    var parts = line.Split(' ');
    if (parts[0] == "mask")
    {
        orMask = 0;
        floatBits.Clear();
        for (int i = 0; i < parts[2].Length; i++)
        {
            ulong bitVal = 1ul << (35 - i);
            switch (parts[2][i])
            {
                case '1':
                    // Set bit
                    orMask |= bitVal;
                    break;
                case 'X':
                    // Floating value
                    floatBits.Add(bitVal);
                    break;
            }
        }
    }
    else
    {
        var match = Regex.Match(line, @"^mem\[(\d+)\] = (\d+)");
        ulong address = ulong.Parse(match.Groups[1].Value) | orMask;
        ulong operand = ulong.Parse(match.Groups[2].Value);

        foreach (ulong a in PermuteAddresses(address, floatBits))
        {
            if (!memory.ContainsKey(a))
            {
                memory.Add(a, 0);
            }
            memory[a] = operand;
        }
    }
}

total = 0;
foreach (var kv in memory)
{
    total += kv.Value;
}
Console.WriteLine($"Q2: {total}");

IEnumerable<ulong> PermuteAddresses(ulong address, List<ulong> floatBits)
{
    // The number of permutations is 2^(num floating bits)
    for (int i = 0; i < Math.Pow(2, floatBits.Count); i++)
    {
        // Set or reset each bit according to whether it's present in the 
        // current value of i
        for (int j = 0; j < floatBits.Count; j++)
        {
            int bitVal = 1 << j;
            // floatBits gives us the value to OR in or ~AND out
            if ((i & bitVal) != 0)
            {
                address |= floatBits[j];
            }
            else
            {
                address &= ~floatBits[j];
            }
        }
        yield return address;
    }
}