using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DFSMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ReadMachineFromFile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine("Hello World!");
        }

        static void ReadMachineFromFile()
        {
            var q = new List<string>();
            var sigma = new List<char>();
            var delta = new List<Transition>();
            var q0 = string.Empty;
            var f = new List<string>();
            using (var file = new StreamReader("transition.txt"))
            {
                string[] lines;
                string line;
                using var secondFile = new StreamReader("initial.txt");
                while ((line = secondFile.ReadLine()) != null)
                {
                    if (line.Contains("start"))
                    {
                        q0 = line.Split("=")[1];
                    }

                    if (line.Contains("sigma"))
                    {
                        sigma.AddRange(line.Split("=")[1].Split(";").Select(s => s.ElementAt(0)));
                    }

                    if (line.Contains("end"))
                    {
                        f.AddRange(line.Split("=")[1].Split(";"));
                    }
                }

                while ((lines = file.ReadLine()?.Split(";")) != null)
                {
                    if (lines.Length != 3)
                    {
                        throw new ArgumentException("Line is not valid.");
                    }

                    delta.Add(new Transition(lines[0], lines[1].ElementAt(0), lines[2]));
                }
            }

            var dfs = new DeterministicFiniteStateMachine(q, sigma, delta, q0, f);

            using (var file = new StreamReader("values.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    dfs.Accepts(line);
                }
            }
        }
    }
}