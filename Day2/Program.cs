using System;
using System.IO;
using System.Linq;

int q1Total = 0;
int q2Total = 0;

foreach (string line in File.ReadAllLines("Input.txt"))
{
    string[] elements = line.Split(new char[] { '-', ' ', ':' },
        StringSplitOptions.RemoveEmptyEntries);
    int min = Convert.ToInt32(elements[0]);
    int max = Convert.ToInt32(elements[1]);
    char passwordChar = elements[2][0];
    string password = elements[3];

    int count = password.Where(x => x == passwordChar).Count();
    if (count >= min && count <= max)
    {
        q1Total++;
    }

    if ((password[min - 1] == passwordChar || password[max - 1] == passwordChar) &&
        !(password[min - 1] == passwordChar && password[max - 1] == passwordChar))
    {
        q2Total++;
    }
}

Console.WriteLine("Q1: " + q1Total);
Console.WriteLine("Q2: " + q2Total);