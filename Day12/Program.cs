using System;
using System.IO;

string[] Input = File.ReadAllLines("Input.txt");

int north = 0;
int east = 0;
char heading = 'E';
string directions = "NESW";

foreach (string line in Input)
{
    int distance = int.Parse(line[1..]);
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

int waypointNorth = 1;
int waypointEast = 10;
north = 0;
east = 0;
heading = 'E';

foreach (string line in Input)
{
    int distance = int.Parse(line[1..]);
    switch (line[0])
    {
        case 'F':
            north += distance * waypointNorth;
            east += distance * waypointEast;
            break;
        case 'N':
            waypointNorth += distance;
            break;
        case 'S':
            waypointNorth -= distance;
            break;
        case 'E':
            waypointEast += distance;
            break;
        case 'W':
            waypointEast -= distance;
            break;
        case 'R':
            (waypointNorth, waypointEast) = DoRotate(waypointNorth, waypointEast, distance);
            break;
        case 'L':
            (waypointNorth, waypointEast) = DoRotate(waypointNorth, waypointEast, 360 - distance);
            break;
        default:
            break;
    }
}

static (int, int) DoRotate(int n, int e, int degrees)
{
    return degrees switch
    {
        90  => (-e,  n),
        180 => (-n, -e),
        270 => ( e, -n),
        _   => throw new Exception("Invalid rotation.")
    };
}

Console.WriteLine($"Q2: {Math.Abs(north) + Math.Abs(east)}");