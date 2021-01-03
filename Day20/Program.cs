using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// Notes:
// The full image is a 12x12 grid.
// Perimeter is 12 + 12 + 10 + 10 = 44.
// Internal tiles which match on all 4 edges = 144 - 44 = 100.
// Edge tiles which match on 3 edges = 10 + 10 + 10 + 10 = 40.
// Corner tiles which match on 2 edges = 4. Matching edges should be adjoining.

string[] input = File.ReadAllLines("Input.txt");
List<int> ids = new();
List<string> edges = new();
// Tile index, edges matching
Dictionary<int, int> matches = new();

for (int i = 0; i < input.Length; i += 12)
{
    string left = "";
    string right = "";

    ids.Add(int.Parse(input[i].Split(" ")[1].Trim(':')));
    edges.Add(input[i + 1]);    // Top
    for (int j = 1; j < 11; j++)
    {
        right += input[i + j][9];
        left = input[i + j][0] + left;
    }
    edges.Add(right);
    edges.Add(new string(input[i + 10].Reverse().ToArray()));   // Bottom
    edges.Add(left);
}

//// Test for unique edges on each tile
//for (int i = 0; i < ids.Count; i++)
//{
//    HashSet<string> check = new();
//    for (int j = 0; j < 4; j++)
//    {
//        // Add returns false if the element is already present
//        if (!check.Add(edges[i * 4 + j]))
//        {
//            System.Diagnostics.Debug.Fail($"Dupe found on tile {ids[i]}");
//        }
//    }
//}

UpdateMatches();
var fourSides = matches.Where(x => x.Value == 4);
foreach (var t in fourSides)
{
    Console.WriteLine($"Tile {ids[t.Key]} has 4 sides matching.");
}

var innerMatches = GetMatches(fourSides);
//foreach (var tileIndex in GetMatches(fourSides))
//{
//    Console.WriteLine($"Tile {ids[tileIndex]} has 4 matches with the other tiles in the set");
//}

List<(int x, int y, int tileIndex)> grid = new();
var firstTileIndex = innerMatches.First();
grid.Add((0, 0, ids[firstTileIndex]));
PlaceSurroundingTiles(0, 0, firstTileIndex);

//do
//{
//    UpdateMatches();
//    int tileIndex = Array.FindIndex(matches, x => x == 0 || x == 1);
//    if (tileIndex == -1)
//    {
//        break;
//    }
//    Flip(tileIndex);
//} while (true);  // TODO: replace with proper validation call

Console.Write("");

void PlaceSurroundingTiles(int x, int y, int tileIndex)
{

}

/// <summary>
/// Reverse all edges of a tile.
/// </summary>
void Flip(int tileIndex)
{
    foreach (int idx in Enumerable.Range(tileIndex * 4, 4))
    {
        edges[idx] = new string(edges[idx].Reverse().ToArray());
    }
}

IEnumerable<int> GetMatches(IEnumerable<KeyValuePair<int, int>> tileset)
{
    foreach (var t in tileset)
    {
        bool match = true;
        // Check this tile's four sides against the other tiles in the set
        for (int i = 0; i < 4; i++)
        {
            // Does the edge only match against itself?
            if (edges.Count(x => x == edges[t.Key * 4 + i]) == 1)
            {
                match = false;
                break;
            }
        }
        if (match)
        {
            yield return t.Key;
        }
    }
}

void UpdateMatches()
{
    for (int i = 0; i < ids.Count; i++)
    {
        Console.WriteLine($"Tile {ids[i]}");
        matches[i] = 0;
        // Each tile edge
        for (int j = 0; j < 4; j++)
        {
            string edge = edges[i * 4 + j];
            if (edges.Count(x => x == edge) - 1 > 0)
            {
                matches[i]++;
            }
        }
        Console.WriteLine($"Tile: {ids[i]}, Edges matched: {matches[i]}");
    }
}