using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

List<Rule> rules = new();
foreach (string line in File.ReadAllLines("Input.txt").TakeWhile(x => x.Length > 0))
{
    rules.Add(new Rule(line));
}

do
{
    foreach (var rule in rules.Where(x => x.IsFinal()))
    {
        foreach (var toUpdate in rules)
        {
            toUpdate.Update(rule);
        }
    }
} while (!rules.First(x => x.Id == "0").IsFinal());

Console.WriteLine($"Q1: {TotalMatches()}");  // 136
Console.WriteLine($"Q2: {TotalMatches(true)}"); // 256

int TotalMatches(bool isQ2 = false)
{
    string regex;
    int total = 0;

    if (isQ2)
    {
        // Combine Rule 8 and Rule 11 for question 2
        // Rule 8 is any number of Rule 42
        string rule42 = MakeRegex(rules, "42");
        string rule8 = $"({rule42})+";
        // Rule 11 is any matching number of rules 42 and 31
        string rule31 = MakeRegex(rules, "31");
        // NB: longest test = 96 characters
        // The minimum required to brute-force match all our tests = 1-4 matches
        string rule11 = $"({rule42}{rule31}|{rule42}{rule42}{rule31}{rule31}|{rule42}{rule42}{rule42}{rule31}{rule31}{rule31}|{rule42}{rule42}{rule42}{rule42}{rule31}{rule31}{rule31}{rule31})";
        // This non-brute-force solution over-matches by 2 for some reason :(
//        string rule11 = $"((?<Open>{rule42})|(?<Content-Open>{rule31}))+(?(Open)(?!))";
        regex = $"^{rule8}{rule11}$";
    }
    else
    {
        regex = $"^{MakeRegex(rules, "0")}$";
    }

    regex = regex.Replace("\"", "").Replace(" ", "");

    foreach (var test in File.ReadAllLines("Input.txt").SkipWhile(x => x.Length > 0).Skip(1))
    {
        if (Regex.IsMatch(test, regex))
        {
            total++;
        }
    }
    return total;
}

static string MakeRegex(List<Rule> rules, string ruleId)
{
    return rules.First(x => x.Id == ruleId).ToString();
}

public class Rule
{
    public string Id { get; }
    public List<string> GroupA { get; set; } = new();
    public List<string> GroupB { get; set; } = new();

    public Rule(string line)
    {
        bool firstGroup = true;
        foreach (string str in line.Split(' '))
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = str.Trim(':');
            }
            else if (str == "|")
            {
                firstGroup = false;
            }
            else if (firstGroup)
            {
                GroupA.Add(str);
            }
            else
            {
                GroupB.Add(str);
            }
        }
    }

    public void Update(Rule r)
    {
        for (int i = 0; i < GroupA.Count; i++)
        {
            if (GroupA[i] == r.Id)
            {
                GroupA[i] = r.ToString();
            }
        }
        for (int i = 0; i < GroupB.Count; i++)
        {
            if (GroupB[i] == r.Id)
            {
                GroupB[i] = r.ToString();
            }
        }
    }

    public bool IsFinal()
    {
        return !(GroupA.Any(x => int.TryParse(x, out _)) ||
            GroupB.Any(x => int.TryParse(x, out _)));
    }

    public override string ToString()
    {
        return GroupB.Count == 0 ?
            string.Join(" ", GroupA) :
            $"({string.Join(" ", GroupA)}|{string.Join(" ", GroupB)})";
    }
}