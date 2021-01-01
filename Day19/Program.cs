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
} while (!rules.First(x => x.Key == "0").IsFinal());

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
        // This non-brute-force solutionover-matches by 2 for some reason :(
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

static string MakeRegex(List<Rule> rules, string ruleId, bool isQ2 = false)
{
    //if (ruleId == "8" && isQ2)
    //{
    //    // For Q2 rule 8 ("8: 42 | 42 8") is any number of rule 42, so 
    //    // this evaluates to "(rule42)+"
    //    string rule42 = MakeRegex(rules, "42");
    //    return $"({rule42})+";
    //}
    //else if (ruleId == "11" && isQ2)
    //{
    //    // Q2 rule 11 ("11: 42 31 | 42 11 31") requires a balanced number 
    //    // of rule 42 and rule 31, e.g. "42 31" or "42 42 31 31". This can 
    //    // be achieved using .NET regex balanced groups. Official.NET docs
    //    // are poor at explaining this construct. Instead see:
    //    // https://www.codeproject.com/articles/21183/in-depth-with-net-regex-balanced-grouping
    //    // Example here: http://regexstorm.net/tester?p=%28%28%3f%3cOpen%3eaba%29%7c%28%3f%3cContent-Open%3ebaaab%29%29%2b%28%3f%28Open%29%28%3f!%29%29&i=aaaabaababaaabbaaabaaaaa
    //    // 
    //    // (Alternatively, we could brute-force the pattern until it is long 
    //    // enough to match all our tests.)
    //    string rule42 = MakeRegex(rules, "42");
    //    string rule31 = MakeRegex(rules, "31");
    //    return $"((?<Open>{rule42})|(?<Content-Open>{rule31}))+(?(Open)(?!))";
    //}
    //else
    //{
        return rules.First(x => x.Key == ruleId).ToString();
    //}
}

public class Rule
{
    public string Key { get; }
    public List<string> GroupA { get; set; } = new();
    public List<string> GroupB { get; set; } = new();

    public Rule(string line)
    {
        bool firstGroup = true;
        foreach (string str in line.Split(' '))
        {
            if (string.IsNullOrEmpty(Key))
            {
                Key = str.Trim(':');
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
            if (GroupA[i] == r.Key)
            {
                GroupA[i] = r.ToString();
            }
        }
        for (int i = 0; i < GroupB.Count; i++)
        {
            if (GroupB[i] == r.Key)
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