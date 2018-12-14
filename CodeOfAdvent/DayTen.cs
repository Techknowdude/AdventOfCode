using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace CodeOfAdvent
{
    public class DayTen : ICodingDay
    {
        public void Run(List<string> inputs)
        {   
            //       0        1   2    3        4   5
            // ex: position=< 7,  0> velocity=<-1,  0>
            var particles = new List<Particle>();

            foreach (var input in inputs)
            {
                particles.Add(new Particle(input));
            }

            var map = new char[Location.HighX][];
            for (int index = 0; index < map.Length; index++)
            {
                map[index] = Enumerable.Repeat(' ', Location.HighY).ToArray();
            }

            while (true)
            {
                PrintMap(map, particles);
                particles.ForEach(p => p.Move());
            }

            Console.WriteLine();
        }

        private void PrintMap(char[][] map, List<Particle> particles)
        {
            //clear the map
            for (int index = 0; index < map.Length; index++)
            {
                map[index] = Enumerable.Repeat(' ', Location.HighY).ToArray();
                var row = map[index];
            }
        }


        protected class Particle
        {
            public Location Location;
            public Location Velocity;

            public Particle(string input)
            {
                var parse = input.Split(new []{ ',', '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                Location = new Location(Convert.ToInt32(parse[1]), Convert.ToInt32(parse[2]));
                Velocity = new Location(Convert.ToInt32(parse[4]), Convert.ToInt32(parse[5]));
            }

            public void Move()
            {
                Location = Location + Velocity;
            }
            public override string ToString()
            {
                return $"P({Location}. {Velocity}";
            }
        }

        protected class Location
        {
            public int X;
            public int Y;
            public static int HighX;
            public static int HighY;

            public Location(int x, int y)
            {
                X = x;
                Y = y;

                HighX = Math.Max(X, HighX);
                HighY = Math.Max(Y, HighY);
            }

            public override string ToString()
            {
                return $"<{X},{Y}>";
            }

            public static Location operator +(Location l1, Location l2)
            {
                return new Location(l1.X+l2.X,l1.Y+l2.Y);
            }
        
        }
    }
}