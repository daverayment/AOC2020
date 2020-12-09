using System;
using System.Linq;
using System.IO;

long[] Input = File.ReadAllLines("Input.txt").Select(x => long.Parse(x)).ToArray();
const int PreambleLength = 25;
long answer = -1;

for (int i = PreambleLength; i < Input.Length; i++)
{
    if (!HasSum(i))
    {
        Console.WriteLine("Q1: " + Input[i]);
        answer = Input[i];
        break;
    }
}

for (int start = 0; start < Input.Length - 1; start++)
{
    long first = Input[start];
    long sum = first;
    long smallest = first;
    long largest = first;
    int end = start + 1;

    do
    {
        long current = Input[end++];
        smallest = Math.Min(smallest, current);
        largest = Math.Max(largest, current);
        sum += current;
    } while (sum < answer);

    if (sum == answer)
    {
        Console.WriteLine($"Q2: {smallest + largest}");
    }
}

bool HasSum(int index)
{
    for (int i = index - PreambleLength; i < index; i++)
    {
        for (int j = index - PreambleLength + 1; j < index; j++)
        {
            if (i == j)
            {
                continue;
            }
            if (Input[index] == Input[i] + Input[j])
            {
                return true;
            }
        }
    }

    return false;
}