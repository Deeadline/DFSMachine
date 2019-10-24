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
        }

        private static void ReadMachineFromFile()
        {
            var q = new List<string>();
            var sigma = new List<char>();
            var delta = new List<Transition>();
            var q0 = string.Empty;
            var f = new List<string>();
            var currentDirectory = Environment.CurrentDirectory.Substring(0,
                Environment.CurrentDirectory.IndexOf("bin", StringComparison.Ordinal));
            var transitionPath = currentDirectory + "transition.txt";
            var initialPath = currentDirectory + "initial.txt";
            var valuesPath = currentDirectory + "values.txt";
            using (var file = new StreamReader(transitionPath))
            {
                string[] lines;
                string line;
                using var secondFile = new StreamReader(initialPath);
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

                    if (!q.Contains(lines[0]))
                    {
                        q.Add(lines[0]);
                    }

                    delta.Add(new Transition(lines[0], lines[1].ElementAt(0), lines[2]));
                }
            }


            var dfs = new DeterministicFiniteStateMachine(q, sigma, delta, q0, f);

            using (var file = new StreamReader(valuesPath))
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