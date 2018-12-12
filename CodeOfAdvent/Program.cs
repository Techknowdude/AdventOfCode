using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeOfAdvent
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialInputs = GetExampleInput();// File.ReadAllLines("InputDay6.txt").ToList();
            new DaySeven().Run(initialInputs);
        }

        private static List<string> GetExampleInput()
        {
            return @"Step C must be finished before step A can begin.
Step C must be finished before step F can begin.
Step A must be finished before step B can begin.
Step A must be finished before step D can begin.
Step B must be finished before step E can begin.
Step D must be finished before step E can begin.
Step F must be finished before step E can begin.".Split(new char[] {'\n'}).ToList();
        }
    }
}
