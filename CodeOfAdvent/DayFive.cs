using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeOfAdvent
{
    internal class DayFive
    {
        const int CapitalOffset = 'a' - 'A';
        public static void Run(List<string> initialInputs)
        {
            var polymer = initialInputs[0];
            var pairs = new List<string>();
            
            for (char character = 'a'; character <= 'z'; character++)
            {
                    pairs.Add(character.ToString()+(char)(character - CapitalOffset));
                    pairs.Add(((char)(character - CapitalOffset)).ToString()+character);
            }

            int len = polymer.Length;
            bool shrinking = true;

            while (shrinking)
            {
                pairs.ForEach(pair =>polymer = polymer.Replace(pair,""));
                shrinking = len > polymer.Length;
                len = polymer.Length;
            }

            // part 1
            Console.WriteLine(polymer.Length);

            var bestLen = int.MaxValue;
            string bestString = String.Empty;
            polymer = initialInputs[0];

            for (char removedCharacter = 'a'; removedCharacter <= 'z'; removedCharacter++)
            {
                var testedPolymer =
                    polymer.Replace(removedCharacter.ToString(), "").Replace(((char) (removedCharacter - CapitalOffset)).ToString(), "");

                shrinking = true;

                while (shrinking)
                {
                    pairs.ForEach(pair => testedPolymer = testedPolymer.Replace(pair, ""));
                    shrinking = len > testedPolymer.Length;
                    len = testedPolymer.Length;
                }

                if (len < bestLen)
                {
                    bestLen = len;
                    bestString = testedPolymer;
                }
            }

            Console.WriteLine(bestString.Length);
        }
    }
}