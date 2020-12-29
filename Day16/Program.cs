using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

States currentState = States.ReadingFieldRanges;
List<TicketField> fields = new();
int scanningErrorRate = 0;
List<int> myTicket = new();
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
                myTicket = line.Split(',').Select(x => int.Parse(x)).ToList();
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
            if (!IsValidValue(ticket[place], fields[f]))
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

foreach (var field in validFields.OrderBy(x => x.Value.Count))
{
    int place = field.Key;
    int validField = field.Value.Single(x => !used.Contains(x));
    used.Add(validField);

    if (fields[validField].Name.StartsWith("departure"))
    {
        // Console.WriteLine(myTicket[place]);
        total *= myTicket[place];
    }
    // Console.WriteLine($"Assigned place {place} to field {validField} ({fields[validField].Name})");
}

Console.WriteLine($"Q2: {total}");

IEnumerable<int> GetInvalidValues(string line)
{
    foreach(int val in line.Split(',').Select(x => int.Parse(x)))
    {
        bool isValid = false;
        foreach (var field in fields)
        {
            if (IsValidValue(val, field))
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

bool IsValidValue(int value, TicketField field)
{
    return (value >= field.Low1 && value <= field.High1) ||
        (value >= field.Low2 && value <= field.High2);
}

public record TicketField(string Name, int Low1, int High1, int Low2, int High2);

enum States { ReadingFieldRanges, ReadingMyTicket, ReadingNearbyTickets };