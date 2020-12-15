using System;
using System.IO;
using System.Text;
using System.Linq;

string[] Input = File.ReadAllLines("Input.txt");
string[] OldSeating = (string[])Input.Clone();
string[] NewSeating = (string[])OldSeating.Clone();
int RowCount = Input.Length;
int RowLength = Input[0].Length;
var Directions = new (int, int)[] { (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1) };

// Q1: Someone sits down if there are no neighbours.
// Someone leaves if there are too many neighbours.
int seatsOccupied = GetOccupiedSeats((x, y) => {
    char seat = OldSeating[y][x];
    int occupied = Directions.Select(dir => CheckDirection(x, y, dir)).Count(x => x == '#');
    return seat == 'L' && occupied == 0 ? '#' :
        seat == '#' && occupied >= 4 ? 'L' :
        seat;
});
Console.WriteLine($"Q1: {seatsOccupied}");

// Q2: As before, but people check any distance when checking for empty or 
// occupied seats. Also, 5 or more is now considered too many neighbours.
OldSeating = (string[])Input.Clone();

seatsOccupied = GetOccupiedSeats((x, y) => {
    char seat = OldSeating[y][x];
    int occupied = Directions.Select(dir => CheckDirection(x, y, dir, checkOneSpaceOnly: false)).Count(x => x == '#');
    return seat == 'L' && occupied == 0 ? '#' :
        seat == '#' && occupied >= 5 ? 'L' :
        seat;
});
Console.WriteLine($"Q2: {seatsOccupied}");

/// <summary>
/// Count the number of occupied seats according to the passed-in rule.
/// </summary>
/// <param name="checkFn">A function which takes the X, Y coordinates of the
/// seat to check and returns what it should be in the next step.</param>
/// <returns>Number of occupied seats.</returns>
int GetOccupiedSeats (Func<int, int, char> checkFn)
{
    bool hasChanged;
    do {
        hasChanged = false;
        for (int y = 0; y < RowCount; y++)
        {
            var row = new StringBuilder(OldSeating[y]);
            for (int x = 0; x < RowLength; x++)
            {
                row[x] = checkFn(x, y);
            }
            NewSeating[y] = row.ToString();
            if (NewSeating[y] != OldSeating[y])
            {
                hasChanged = true;
            }
        }
        OldSeating = (string[])NewSeating.Clone();
    } while (hasChanged);

    return NewSeating.Select(row => row.Count(x => x == '#')).Sum();
}

char CheckDirection(int x, int y, (int dx, int dy) dir, bool checkOneSpaceOnly = true)
{
    do
    {
        x += dir.dx;
        y += dir.dy;
        if (x < 0 || y < 0 || x >= RowLength || y >= RowCount)
        {
            return ' ';
        }
        char seat = OldSeating[y][x];
        if (seat == '#' || seat == 'L')
        {
            return seat;
        }
        if (checkOneSpaceOnly)
        {
            return ' ';
        }
    } while(true);
}