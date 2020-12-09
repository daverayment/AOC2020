using System;
using System.Collections.Generic;
using System.IO;

var Input = File.ReadAllLines("Input.txt");
int acc;
int pc;

DoesFinish();
Console.WriteLine("Q1: " + acc);

int lastChanged = -1;
do
{
    if (lastChanged >= 0)
    {
        // Revert change from previous iteration
        ChangeInstruction(lastChanged);
    }

    // Change 1 nop to jmp or jmp to nop
    for (int i = lastChanged + 1; i < Input.Length; i++)
    {
        if (Input[i].StartsWith("jmp") || Input[i].StartsWith("nop"))
        {
            ChangeInstruction(i);
            lastChanged = i;
            break;
        }
    }
} while (!DoesFinish());

Console.WriteLine("Q2: " + acc);

void ChangeInstruction(int address)
{
    Input[address] = Input[address].StartsWith("jmp") ?
        Input[address].Replace("jmp", "nop") :
        Input[address].Replace("nop", "jmp");
}

bool DoesFinish()
{
    pc = 0;
    acc = 0;
    HashSet<int> seen = new();

    while (!seen.Contains(pc) && pc < Input.Length)
    {
        seen.Add(pc);
        string[] parts = Input[pc].Split(" ");
        switch (parts[0])
        {
            case "acc":
                acc += int.Parse(parts[1]);
                pc++;
                break;
            case "jmp":
                pc += int.Parse(parts[1]);
                break;
            case "nop":
                pc++;
                break;
        }
    }

    return pc == Input.Length;
}