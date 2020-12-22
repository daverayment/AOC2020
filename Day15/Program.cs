using System;
using System.Collections.Generic;

int[] Input = { 12,1,16,3,11,0 };

Console.WriteLine($"Q1: {Play(2020)}");
Console.WriteLine($"Q2: {Play(30000000)}");

int Play(int turns)
{
    // Number and turn last seen
    Dictionary<int, int> seen = new();
    
    int turn = 1;
    foreach (int num in Input)
    {
        seen.Add(num, turn++);
    }

    int nextNum = 0;
    int lastNum;
    do
    {
        lastNum = nextNum;

        if (seen.ContainsKey(nextNum))
        {
            int temp = nextNum;
            nextNum = turn - seen[nextNum];
            seen[temp] = turn;
        }
        else
        {
            seen.Add(nextNum, turn);
            nextNum = 0;
        }
        turn++;
    } while (turn <= turns);

    return lastNum;
}