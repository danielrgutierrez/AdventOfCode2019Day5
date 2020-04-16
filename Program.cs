using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019Day5
{
    enum OpCode { Add = 1, Multiply = 2, End = 99 }

    class Program
    {
        static void Main(string[] args)
        {
            var inputPart1 = new List<int>();
            try
            {
                using (StreamReader sr = new StreamReader("input.txt"))
                {
                    string? strProgram;
                    while ((strProgram = sr.ReadLine()) != null)
                    {
                        inputPart1 = strProgram.Split(',').Select(int.Parse).ToList();
                    }
                    var inputPart2 = CopyIntProgram(in inputPart1);
                    // Get Part 1 Answer
                    SetNounAndVerb(ref inputPart1);
                    ProcessIntCodeProgram(ref inputPart1);
                    Console.WriteLine($"Part 1: The value left at position 0 is {inputPart1[0]}.");
                    // Get Part2 Answer
                    Tuple<int, int>? nounAndVerb = FindNounAndVerb(ref inputPart2);
                    if (nounAndVerb != null)
                    {
                        Console.WriteLine($"Part 2: The noun is {nounAndVerb.Item1}.\nThe verb is {nounAndVerb.Item2}.\n100 * noun + verb = {100 * nounAndVerb.Item1 + nounAndVerb.Item2}.");
                    }
                    else
                    {
                        Console.WriteLine("Could not find the noun and verb for that target.");
                    }
                }
            }
            catch (IOException e)
            {
                Console.Write($"Error reading the file: {e.Message}");
            }
        }

        static void ProcessIntCodeProgram(ref List<int> program)
        {
            var index = 0;
            var programFinished = false;
            while (index < program.Count && !programFinished)
            {
                switch ((OpCode)program[index])
                {
                    case OpCode.Add:
                        program[program[index + 3]] = program[program[index + 1]] + program[program[index + 2]];
                        break;
                    case OpCode.Multiply:
                        program[program[index + 3]] = program[program[index + 1]] * program[program[index + 2]];
                        break;
                    case OpCode.End:
                    default:
                        programFinished = true;
                        break;
                }
                index += 4;
            }
        }

        static void PrintIntCodeProgram(in List<int> program)
        {
            for (int i = 0; i < program.Count; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    Console.WriteLine();
                }
                Console.Write($"{program[i]},");
            }
            Console.WriteLine();
        }

        static void SetNounAndVerb(ref List<int> program, int noun = 12, int verb = 2)
        {
            program[1] = noun;
            program[2] = verb;
        }

        static Tuple<int, int>? FindNounAndVerb(ref List<int> program, int target = 19690720)
        {
            var originalProgram = CopyIntProgram(in program);
            var foundNoun = 0;
            var foundVerb = 0;
            bool foundNounAndVerb = false;
            for (int noun = 0; noun <= 99 && !foundNounAndVerb; noun++)
            {
                for (int verb = 0; verb <= 99 && !foundNounAndVerb; verb++)
                {
                    SetNounAndVerb(ref program, noun, verb);
                    ProcessIntCodeProgram(ref program);
                    if (program[0] == target)
                    {
                        foundNounAndVerb = true;
                        foundNoun = noun;
                        foundVerb = verb;
                    }
                    program = CopyIntProgram(in originalProgram);
                }
            }
            return foundNounAndVerb ? Tuple.Create(foundNoun, foundVerb) : null;
        }

        static List<int> CopyIntProgram(in List<int> program)
        {
             var newProgram = new List<int>();
             foreach(var code in program)
             {
                 newProgram.Add(code);
             }

             return newProgram;
        }
    }
}
