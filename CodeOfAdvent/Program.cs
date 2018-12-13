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
            bool testing = true;

            var initialInputs = testing ? GetExampleInput() : File.ReadAllLines("InputDay9.txt").ToList();
            new DayNine().Run(initialInputs);
        }

        private static List<string> GetExampleInput()
        {
            return @"10 players; last marble is worth 1618 points: high score is 8317".Split(new char[] {' '}).ToList();
        }
    }
}
