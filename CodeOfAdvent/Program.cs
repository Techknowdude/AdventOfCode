﻿using System.Collections.Generic;
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

            var initialInputs = testing ? GetExampleInput() : File.ReadAllLines("InputDay11.txt").ToList();
            new DayEleven().Run(initialInputs);
        }

        private static List<string> GetExampleInput()
        {
            return new List<string>() { @"8", };
        }
    }
}
