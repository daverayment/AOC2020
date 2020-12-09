using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

List<int> Input = File.ReadAllLines("Input.txt").Select(x => Convert.ToInt32(x)).ToList();

Console.WriteLine($"Q1: {TwoNums(2020)}");
Console.WriteLine($"Q2: {ThreeNums(2020)}");

int TwoNums(int total)
{
    int num = Input.Where(x => Input.Contains(total - x)).FirstOrDefault();
    return num is default(int) ? num : num * (total - num);
}

int ThreeNums(int total)
{
    int res = Input.First(x => TwoNums(total - x) != default);
    return res * TwoNums(total - res);
}