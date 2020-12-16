using System;
using System.IO;

string[] Input = File.ReadAllLines("Input.txt");

int north = 0;
int east = 0;
char heading = 'E';
string directions = "NESW";

foreach (string line in Input)
{
    int distance = int.Parse(line.Substring(1));
    char command = line[0];
    if (command == 'F')
    {
        command = heading;
    }
    switch (command)
    {
        case 'N':
            north += distance;
            break;
        case 'S':
            north -= distance;
            break;
        case 'E':
            east += distance;
            break;
        case 'W':
            east -= distance;
            break;
        case 'R':
            int rotate = distance / 90;
            heading = directions[(directions.IndexOf(heading) + rotate) % 4];
            break;
        case 'L':
            rotate = 4 - distance / 90;
            heading = directions[(directions.IndexOf(heading) + rotate) % 4];
            break;
        default:
            break;
    }
}

Console.WriteLine($"Q1: {Math.Abs(north) + Math.Abs(east)}");
