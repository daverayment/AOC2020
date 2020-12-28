using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

States currentState = States.ReadingFieldRanges;
List<TicketField> fields = new();
int scanningErrorRate = 0;
string myTicket = "";
List<int[]> validTickets = new();

foreach (string line in File.ReadAllLines("Input.txt"))
{
    switch (currentState)
    {
        case States.ReadingFieldRanges:
            if (line.Length == 0)
            {
                currentState = States.ReadingMyTicket;
            }
            else
            {
                var m = Regex.Match(line, @"(.+): (\d+)-(\d+) or (\d+)-(\d+)");
                fields.Add(new TicketField(m.Groups[1].Value,
                    int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value),
                    int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value))
                );
            }
            break;

        case States.ReadingMyTicket:
            if (line == "your ticket:")
            {
                continue;
            }
            if (line.Length == 0)
            {
                currentState = States.ReadingNearbyTickets;
            }
            else
            {
                myTicket = line;
            }

            break;

        case States.ReadingNearbyTickets:
            if (line == "nearby tickets:")
            {
                continue;
            }
            int errors = GetInvalidValues(line).Sum();
            scanningErrorRate += errors;
            if (errors == 0)
            {
                validTickets.Add(line.Split(',').Select(x => int.Parse(x)).ToArray());
            }
            break;
    }
}

Console.WriteLine("Q1: " + scanningErrorRate);

// Q2: determine order of fields based on the valid tickets

// Ticket place and the field indexes in which it is valid to appear
Dictionary<int, List<int>> validFields = new();

// All places in all tickets
int numPlaces = validTickets[0].Length;
for (int place = 0; place < numPlaces; place++)
{
    validFields.Add(place, new List<int>());

    // Do all the tickets fit this field's ranges?
    for (int f = 0; f < fields.Count; f++)
    {
        bool valid = true;
        foreach (var ticket in validTickets)
        {
            int val = ticket[place];
            if (!((val >= fields[f].Low1 && val <= fields[f].High1) ||
                (val >= fields[f].Low2 && val <= fields[f].High2)))
            {
                valid = false;
                break;
            }
        }
        if (valid)
        {
            // Every ticket fits the field in this place
            validFields[place].Add(f);
        }
    }
}

List<int> used = new();
long total = 1;
int[] myTicketVals = myTicket.Split(',').Select(x => int.Parse(x)).ToArray();

foreach (var field in validFields.OrderBy(x => x.Value.Count))
{
    int place = field.Key;
    int validField = field.Value.Single(x => !used.Contains(x));
    used.Add(validField);

    if (fields[validField].Name.StartsWith("departure"))
    {
        Console.WriteLine(myTicketVals[place]);
        total *= myTicketVals[place];
    }
    Console.WriteLine($"Assigned place {place} to field {validField} ({fields[validField].Name})");
}

Console.WriteLine($"Q2: {total}");

IEnumerable<int> GetInvalidValues(string line)
{
    foreach(int val in line.Split(',').Select(x => int.Parse(x)))
    {
        bool isValid = false;
        foreach (var field in fields)
        {
            if ((val >= field.Low1 && val <= field.High1) ||
                (val >= field.Low2 && val <= field.High2))
            {
                isValid = true;
                break;
            }
        }
        if (!isValid)
        {
            yield return val;
        }
    }
    yield break;
}

public record TicketField(string Name, int Low1, int High1, int Low2, int High2);

enum States { ReadingFieldRanges, ReadingMyTicket, ReadingNearbyTickets };