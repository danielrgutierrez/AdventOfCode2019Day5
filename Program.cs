using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019Day5
{
    enum OpCode { Add = 1, Multiply = 2, Store = 3, Output = 4, End = 99, }
    enum OpCodeMode { Position = 0, Immediate = 1 }

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
                    ProcessIntCodeProgram(ref inputPart1);
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
                (index, programFinished) = ProcessOpCode(ref program, program[index], index, programFinished);
            }
        }

        static (int index, bool programFinished) ProcessOpCode(ref List<int> program, int possibleOpCode, int index, bool programFinished)
        {
            var opCode = (OpCode)possibleOpCode == OpCode.End ? OpCode.End : (OpCode)(possibleOpCode % 10);
            if ((OpCode)possibleOpCode == OpCode.End)
            {
                return (index + 1, true);
            }
            switch (opCode)
            {
                case OpCode.Add:
                    {
                        var firstOperandMode = (OpCodeMode)((possibleOpCode / 100) % 10);
                        var secondOperandMode = (OpCodeMode)((possibleOpCode / 1000) % 10);
                        var sum = 0; ;
                        if (firstOperandMode == OpCodeMode.Position)
                        {
                            sum += program[program[index + 1]];
                        }
                        else if (firstOperandMode == OpCodeMode.Immediate)
                        {
                            sum += program[index + 1];
                        }

                        if (secondOperandMode == OpCodeMode.Position)
                        {
                            sum += program[program[index + 2]];
                        }
                        else if (secondOperandMode == OpCodeMode.Immediate)
                        {
                            sum += program[index + 2];
                        }
                        program[program[index + 3]] = sum;
                        return (index + 4, false);
                    }
                case OpCode.Multiply:
                    {
                        var firstOperandMode = (OpCodeMode)((possibleOpCode / 100) % 10);
                        var secondOperandMode = (OpCodeMode)((possibleOpCode / 1000) % 10);
                        var product = 1;
                        if (firstOperandMode == OpCodeMode.Position)
                        {
                            product *= program[program[index + 1]];
                        }
                        else if (firstOperandMode == OpCodeMode.Immediate)
                        {
                            product *= program[index + 1];
                        }
                        if (secondOperandMode == OpCodeMode.Position)
                        {
                            product *= program[program[index + 2]];
                        }
                        else if (secondOperandMode == OpCodeMode.Immediate)
                        {
                            product *= program[index + 2];
                        }
                        program[program[index + 3]] = product;
                        return (index + 4, false);
                    }
                case OpCode.Store:
                    Console.Write($"Input for memory location {program[index + 1]}: ");
                    var input = Console.ReadLine();
                    if (Int32.TryParse(input, out int userInput))
                    {
                        program[program[index + 1]] = userInput;
                    }
                    return (index + 2, false);
                case OpCode.Output:
                    {
                        var firstOperandMode = (OpCodeMode)((possibleOpCode / 100) % 10);
                        var output = firstOperandMode == OpCodeMode.Immediate ? program[index + 1] : program[program[index + 1]];
                        Console.WriteLine($"Output for memory location {program[index + 1]}: {output}");
                        return (index + 2, false);
                    }
                case OpCode.End:
                default:
                    return (index + 1, true);
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

        static List<int> CopyIntProgram(in List<int> program)
        {
            var newProgram = new List<int>();
            foreach (var code in program)
            {
                newProgram.Add(code);
            }

            return newProgram;
        }
    }
}
