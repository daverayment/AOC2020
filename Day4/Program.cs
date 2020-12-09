using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

var ValidFields = new List<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", "cid" };
var ValidEyeColours = new List<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
string[] Input = File.ReadAllLines("Input.txt");

Console.WriteLine($"Q1: {ValidPassports()}.");
Console.WriteLine($"Q2: {ValidPassports(strict: true)}.");

int ValidPassports(bool strict = false)
{
    var fieldsPresent = new List<string>(new [] { "cid" });
    int validPassports = 0;

    for (int i = 0; i < Input.Length; i++)
    {
        // Lines are composed of "<name>:<value>" pairs split by spaces
        string[] nvPairs = Input[i].Split(' ');
        foreach (string pair in nvPairs)
        {
            string[] nv = pair.Split(':');
            if (ValidFields.Contains(nv[0]) && !fieldsPresent.Contains(nv[0]))
            {
                bool valueOk = false;
                if (strict)
                {
                    switch (nv[0])
                    {
                        case "byr":
                            valueOk = int.TryParse(nv[1], out int year) && 
                                year >= 1920 && year <= 2002;
                            break;
                        case "iyr":
                            valueOk = int.TryParse(nv[1], out year) && 
                                year >= 2010 && year <= 2020;
                            break;
                        case "eyr":
                            valueOk = int.TryParse(nv[1], out year) && 
                                year >= 2020 && year <= 2030;
                            break;
                        case "hgt":
                            var match = Regex.Match(nv[1], "^([0-9]+)(cm|in)");
                            if (match.Success)
                            {
                                int height = int.Parse(match.Groups[1].Value);
                                valueOk = match.Groups[2].Value == "cm" ? 
                                    height >= 150 && height <= 193 :
                                    height >= 59 && height <= 76;
                            }
                            break;
                        case "hcl":
                            valueOk = Regex.Match(nv[1], "^#([0-9]|[a-f|A-F]){6}$").Success;
                            break;
                        case "ecl":
                            valueOk = ValidEyeColours.Contains(nv[1]);
                            break;
                        case "pid":
                            valueOk = Regex.Match(nv[1], "^[0-9]{9}$").Success;
                            break;
                        case "cid":
                            valueOk = true;
                            break;
                    }
                }
                if (valueOk || !strict)
                {
                    fieldsPresent.Add(nv[0]);
                }
            }
        }

        // Validate a passport if we have come across a blank line or EOF
        bool isLastLine = i == Input.Length - 1;
        if (Input[i].Trim() == string.Empty || isLastLine)
        {
            if (fieldsPresent.Count == 8)
            {
                validPassports++;
            }
            
            fieldsPresent.Clear();
            fieldsPresent.Add("cid");
        }
    }

    return validPassports;
}