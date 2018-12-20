using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace CodeOfAdvent
{
    public class DayEleven : ICodingDay
    {
        protected const int MaxX = 300-2;
        protected const int MaxY = 300-2;
        protected const int MinX = 1;
        protected const int MinY = 1;

        public void Run(List<string> inputs)
        {
            int serialNo = Convert.ToInt32(inputs[0]);

            DoTests();
            int highestPower = Int32.MinValue;
            int bestX = 0;
            int bestY = 0;

            for (int x = MinX; x < MaxX; x++)
            {
                for (int y = MinY; y < MaxY; y++)
                {
                    int powerAtPosition = GetPowerInArea(x, y, serialNo, 3);
                    if (powerAtPosition > highestPower)
                    {
                        bestX = x;
                        bestY = y;
                        highestPower = powerAtPosition;
                    }
                }
            }
            Console.WriteLine(highestPower);

            /*--- Part Two ---
            You discover a dial on the side of the device; it seems to let you select a square of any size, not just 3x3. Sizes from 1x1 to 300x300 are supported.

            Realizing this, you now must find the square of any size with the largest total power. Identify this square by including its size as a third parameter after the top-left coordinate: a 9x9 square with a top-left corner of 3,5 is identified as 3,5,9.

            For example:

            For grid serial number 18, the largest total square (with a total power of 113) is 16x16 and has a top-left corner of 90,269, so its identifier is 90,269,16.
            For grid serial number 42, the largest total square (with a total power of 119) is 12x12 and has a top-left corner of 232,251, so its identifier is 232,251,12.
            */
            highestPower = Int32.MinValue;

            for (int x = MinX; x < MaxX; x++)
            {
                for (int y = MinY; y < MaxY; y++)
                {
                    int maxSize = Math.Min(MaxX - x, MaxY - y);

                    for (int size = 1; size < maxSize; size++)
                    {
                        int powerAtPosition = GetPowerInArea(x, y, serialNo, size);
                        if (powerAtPosition > highestPower)
                        {
                            bestX = x;
                            bestY = y;
                            highestPower = powerAtPosition;
                        }
                    }
                }
            }

            Console.WriteLine(highestPower);
        }


        private int GetPowerInArea(int x, int y, int serialNo, int squareSize)
        {
            int power = 0;
            for (int xLoc = x; xLoc < x+squareSize; xLoc++)
            {
                for (int yLoc = y; yLoc < y+squareSize; yLoc++)
                {
                    power += GetPowerLevel(xLoc, yLoc, serialNo);
                }
            }
            return power;
        }

        public int GetPowerLevel(int x, int y, int serialNo)
        {
            /*
                Find the fuel cell's rack ID, which is its X coordinate plus 10.
                Begin with a power level of the rack ID times the Y coordinate.
                Increase the power level by the value of the grid serial number (your puzzle input).
                Set the power level to itself multiplied by the rack ID.
                Keep only the hundreds digit of the power level (so 12345 becomes 3; numbers with no hundreds digit become 0).
                Subtract 5 from the power level.
                
                For example, to find the power level of the fuel cell at 3,5 in a grid with serial number 8:

                The rack ID is 3 + 10 = 13.
                The power level starts at 13 * 5 = 65.
                Adding the serial number produces 65 + 8 = 73.
                Multiplying by the rack ID produces 73 * 13 = 949.
                The hundreds digit of 949 is 9.
                Subtracting 5 produces 9 - 5 = 4.

                Fuel cell at  122,79, grid serial number 57: power level -5.
                Fuel cell at 217,196, grid serial number 39: power level  0.
                Fuel cell at 101,153, grid serial number 71: power level  4.
             */
            int rackID = x + 10;
            int two = rackID*y;
            int three = two + serialNo;
            int four = three*rackID;
            int five = (four/100)%10;
            int six = five - 5;

            return six;
        }

        private void DoTests()
        {
            if (GetPowerLevel(3, 5, 8) != 4)
            {
                throw new Exception("Fail");
            }
            if (GetPowerLevel(122, 79, 57) != -5)
            {
                throw new Exception("Fail");
            }
            if (GetPowerLevel(217, 196, 39) != 0)
            {
                throw new Exception("Fail");
            }
            if (GetPowerLevel(101, 153, 71) != 4)
            {
                throw new Exception("Fail");
            }
            /*
                Your goal is to find the 3x3 square which has the largest total power. The square must be entirely within the 300x300 grid. Identify this square using the X,Y coordinate of its top-left fuel cell. For example:

                For grid serial number 18, the largest total 3x3 square has a top-left corner of 33,45 (with a total power of 29); these fuel cells appear in the middle of this 5x5 region:

                -2  -4   4   4   4
                -4   4   4   4  -5
                 4   3   3   4  -4
                 1   1   2   4  -3
                -1   0   2  -5  -2
                For grid serial number 42, the largest 3x3 square's top-left is 21,61 (with a total power of 30); they are in the middle of this region:

                -3   4   2   2   2
                -4   4   3   3   4
                -5   3   3   4  -4
                 4   3   3   4  -3
                 3   3   3  -5  -1
             */
            if (GetPowerInArea(33, 45, 18, 3) != 29)
            {
                throw new Exception("Fail");
            }
            if (GetPowerInArea(21, 61, 42, 3) != 30)
            {
                throw new Exception("Fail");
            }
            //For grid serial number 18, the largest total square (with a total power of 113) is 16x16 and has a top-left corner of 90,269, so its identifier is 90,269,16.
            //For grid serial number 42, the largest total square (with a total power of 119) is 12x12 and has a top - left corner of 232,251, so its identifier is 232,251,12.
            if (GetPowerInArea(90, 269, 18, 16) != 113)
            {
                throw new Exception("Fail");
            }
            if (GetPowerInArea(232, 251, 42, 12) != 119)
            {
                throw new Exception("Fail");
            }
        }
    }
}