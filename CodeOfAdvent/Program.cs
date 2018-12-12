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
            bool testing = false;

            var initialInputs = testing ? GetExampleInput() : File.ReadAllLines("InputDay8.txt").ToList();
            new DayEight().Run(initialInputs);
        }

        private static List<string> GetExampleInput()
        {
            return @"2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2".Split(new char[] {' '}).ToList();
        }
    }
}
