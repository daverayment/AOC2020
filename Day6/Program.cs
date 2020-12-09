using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

string[] Input = File.ReadAllLines("Input.txt");

var answered = new HashSet<char>();
int total = 0;

for (int i = 0; i < Input.Length; i++)
{
    foreach (char ch in Input[i])
    {
        if (!answered.Contains(ch))
        {
            answered.Add(ch);
        }
    }

    // Add to total at end of each passenger group
    if (Input[i].Length == 0 || i == Input.Length - 1)
    {
        total += answered.Count;
        answered.Clear();
    }                
}

Console.WriteLine("Q1: " + total);

string allAnswered = "";
total = 0;

for (int i = 0; i < Input.Length; i++)
{
    if (i == 0 || Input[i - 1].Length == 0)
    {
        // Add all answers from first line of new group
        allAnswered = Input[i];
    }
    else
    {
        // Only keep characters which are in the line
        if (Input[i].Length > 0)
        {
            allAnswered = new string(
                allAnswered.Where(x => Input[i].Contains(x)).ToArray());
        }

        // Total up the remaining answers at the end of a group
        if (Input[i].Length == 0 || i == Input.Length - 1)
        {
            total += allAnswered.Length;
            allAnswered = "";
        }
    }
}

Console.WriteLine("Q2: " + total);