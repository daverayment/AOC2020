using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

// The 'can contain' collection. Key = 'child' bag, Value = 'parent/container' bag.
var bags = new Dictionary<string, List<string>>();
// The 'contained by' collection. Key = 'parent' bag, Value = 'child' bag.
var containedBy = new Dictionary<string, List<Bag>>();

foreach (string line in File.ReadAllLines("Input.txt"))
{
    string bagName = line.Split(" bags contain ")[0].Trim();
    var inside = new List<Bag>();
    foreach (Match match in Regex.Matches(line, "(\\d+)\\s([^,.]+) bags?[,.]"))
    {
        inside.Add(new Bag { Name = match.Groups[2].Value, Number = int.Parse(match.Groups[1].Value) } );

        var childBag = match.Groups[2].Value;
        if (!bags.ContainsKey(childBag))
        {
            bags.Add(childBag, new());
        }
        bags[childBag].Add(bagName);
    }
    containedBy.Add(bagName, inside);
}

var containers = new HashSet<string>();
AddBags("shiny gold", bags, containers);
Console.WriteLine("Q1: " + containers.Count);
Console.WriteLine("Q2: " + CountChildBags("shiny gold", containedBy));

long CountChildBags(string parentBag, Dictionary<string, List<Bag>> bags)
{
    long currentCount = 0;

    // Each of the child bags contains multiple other bags
    foreach (Bag bag in bags[parentBag])
    {
        // Count the child bags
        currentCount += bag.Number;
        // And the bags contained within it/them
        long childTotal = CountChildBags(bag.Name, bags);
        currentCount += bag.Number * childTotal;
    }

    return currentCount;
}

void AddBags(string childBag, Dictionary<string, List<string>> bags, HashSet<string> containers)
{
    if (bags.ContainsKey(childBag))
    {
        var parents = bags[childBag];
        containers.UnionWith(bags[childBag]);
        bags.Remove(childBag);
        foreach (string b in parents)
        {
            AddBags(b, bags, containers);
        }
    }
}

class Bag
{
    public string Name { get; set; }
    public int Number { get; set; } = 0;
}