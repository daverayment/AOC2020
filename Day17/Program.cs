using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
 
const int NumCycles = 6;
const int XLen = NumCycles * 2 + 8;
const int YLen = XLen;
const int ZLen = NumCycles * 2 + 1;
const int WLen = ZLen;

// X, Y, Z, W
bool[,,,] cells = new bool[XLen, YLen, ZLen, WLen];
bool[,,,] nextCells = (bool[,,,])cells.Clone();

ReadInput();
int totalActive = 0;

for (int cycle = 1; cycle <= NumCycles; cycle++)
{
    totalActive = OneSlice(NumCycles, true);
    cells = (bool[,,,])nextCells.Clone();
}

Console.WriteLine("Q1: " + totalActive);

// Q2
ReadInput();

for (int cycle = 1; cycle <= NumCycles; cycle++)
{
    totalActive = 0;

    for (int w = 0; w < WLen; w++)
    {
        totalActive += OneSlice(w, false);
    }

    cells = (bool[,,,])nextCells.Clone();
}

Console.WriteLine("Q2: " + totalActive);

void ReadInput()
{
    string[] input = File.ReadAllLines("Input.txt");
    Array.Clear(cells, 0, cells.Length);

    for (int y = 0; y < input.Length; y++)
    {
        for (int x = 0; x < input[0].Length; x++)
        {
            cells[x + NumCycles, y + NumCycles, 0 + NumCycles, 0 + NumCycles] = input[y][x] == '#';
        }
    }
}

int OneSlice(int w, bool is3D)
{
    int totalActive = 0;

    for (int z = 0; z < ZLen; z++)
    {
        for (int y = 0; y < YLen; y++)
        {
            for (int x = 0; x < XLen; x++)
            {
                int activeNeighbours = GetActiveNeighbourCells(x, y, z, w, is3D);
                // If a cube is active and exactly 2 or 3 of its neighbors are also
                // active, the cube remains active. Otherwise, the cube becomes inactive.
                if (cells[x, y, z, w])
                {
                    nextCells[x, y, z, w] = activeNeighbours == 2 || activeNeighbours == 3;
                }
                else
                {
                    // If a cube is inactive but exactly 3 of its neighbors are active, the
                    // cube becomes active. Otherwise, the cube remains inactive.
                    nextCells[x, y, z, w] = activeNeighbours == 3;
                }
                totalActive += nextCells[x, y, z, w] ? 1 : 0;
            }
        }
    }

    return totalActive;
}

int GetActiveNeighbourCells(int x, int y, int z, int w, bool is3D)
{
    return is3D ?
        GetNeighbourCells(x, y, z, w)
            .Where(c1 => c1.w == w)
            .Count(c2 => cells[c2.x, c2.y, c2.z, c2.w]) : 
        GetNeighbourCells(x, y, z, w)
            .Count(c => cells[c.x, c.y, c.z, c.w]);
}

IEnumerable<(int x, int y, int z, int w)> GetNeighbourCells(int x, int y, int z, int w)
{
    for (int nw = w - 1; nw <= w + 1; nw++)
    {
        for (int nz = z - 1; nz <= z + 1; nz++)
        {
            for (int ny = y - 1; ny <= y + 1; ny++)
            {
                for (int nx = x - 1; nx <= x + 1; nx++)
                {
                    // Must be in bounds and not the cell passed in
                    if (nx >= 0 && ny >= 0 && nz >= 0 && nw >= 0 &&
                        nx < XLen && ny < YLen && nz < ZLen && nw < WLen &&
                        !(nx == x && ny == y && nz == z && nw == w))
                    {
                        yield return (nx, ny, nz, nw);
                    }
                }
            }
        }
    }
}