using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeOfAdvent
{
    public class DaysOneToThree : ICodingDay
    {
        public void Run(List<string> inputs)
        {

        }

        private static void DayThree(List<string> initialInputs)
        {
            char[][] TextileMap = null;
            List<ClaimInfo> claims = new List<ClaimInfo>();

            foreach (var initialInput in initialInputs)
            {
                var split = initialInput.Split(new[] { ' ', '#', '@', ',', ':', 'x', });
                var claim = new ClaimInfo(split);
                claims.Add(claim);
            }

            TextileMap = new char[ClaimInfo.HighestY][];
            for (int row = 0; row < ClaimInfo.HighestY; row++)
            {
                TextileMap[row] = Enumerable.Repeat('.', ClaimInfo.HighestX).ToArray();
            }

            foreach (var claimInfo in claims)
            {
                for (int row = claimInfo.X; row < claimInfo.X + claimInfo.Width; row++)
                {
                    for (int column = claimInfo.Y; column < claimInfo.Y + claimInfo.Height; column++)
                    {
                        if (TextileMap[column][row] == '.')
                        {
                            TextileMap[column][row] = 'O';
                        }
                        else if (TextileMap[column][row] == 'O')
                        {
                            TextileMap[column][row] = 'X';
                        }
                    }
                }
            }

            var overlap = TextileMap.Sum(col => col.Count(item => item == 'X'));

            bool badClaim = false;

            foreach (var claimInfo in claims)
            {
                badClaim = false;
                for (int row = claimInfo.X; !badClaim && row < claimInfo.X + claimInfo.Width; row++)
                {
                    for (int column = claimInfo.Y; !badClaim && column < claimInfo.Y + claimInfo.Height; column++)
                    {
                        if (TextileMap[column][row] == 'X')
                        {
                            badClaim = true;
                        }
                    }
                }
                if (!badClaim)
                {
                    Console.WriteLine("Claim #" + claimInfo.Number);
                    break;
                }
            }
        }

        struct ClaimInfo
        {
            public static int HighestX = 0;
            public static int HighestY = 0;
            public int Number;
            public int X;
            public int Y;
            public int Width;
            public int Height;

            public ClaimInfo(string[] split)
            {
                Number = Convert.ToInt32(split[1]);
                X = Convert.ToInt32(split[4]);
                Y = Convert.ToInt32(split[5]);
                Width = Convert.ToInt32(split[7]);
                Height = Convert.ToInt32(split[8]);

                if (HighestX < X + Width)
                    HighestX = X + Width;

                if (HighestY < Y + Height)
                    HighestY = Y + Height;
            }
        }

        private static void DayTwo(List<string> initialInputs)
        {
            bool itemFound = false;
            int dif = 0;
            int difLocation = -1;

            // check each item against all other items
            for (int inputIndex = 0; !itemFound && inputIndex < initialInputs.Count; inputIndex++)
            {
                var comparedInput = initialInputs[inputIndex].ToCharArray();
                // get each other item
                for (int index = inputIndex + 1; !itemFound && index < initialInputs.Count; index++)
                {
                    // reset the dif flag to check for when we find more than one dif
                    dif = 0;
                    if (index >= initialInputs.Count) break;

                    var input = initialInputs[index].ToCharArray();
                    if (comparedInput.Length == input.Length)
                    {
                        for (int charIndex = 0; charIndex < input.Length; charIndex++)
                        {
                            if (comparedInput[charIndex] != input[charIndex])
                            {
                                if (dif > 1)
                                {
                                    break;
                                }
                                else
                                {
                                    difLocation = charIndex;
                                    dif++;
                                }
                            }
                        }

                        if (dif <= 1)
                        {
                            Console.WriteLine("Correct ID is " + initialInputs[index].Remove(difLocation, 1));
                        }
                    }
                }
            }
        }

        private void DayOne(string[] args)
        {
            var currentFrequency = 0;
            var values = new List<int>();
            var initialInputs = File.ReadAllLines(args[0]).Select(a => Convert.ToInt32(a)).ToList();
            bool found = false;
            do
            {
                var inputs = initialInputs.ToList();

                foreach (var input in inputs)
                {
                    currentFrequency += input;
                    if (values.Contains(currentFrequency))
                    {
                        Console.WriteLine("Found repeated value: " + currentFrequency);
                        found = true;
                        break;
                    }
                    else
                    {
                        values.Add(currentFrequency);
                    }
                }
            } while (!found);

            Console.WriteLine("Scan complete.");
        }

    }
}