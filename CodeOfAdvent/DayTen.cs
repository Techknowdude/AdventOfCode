using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace CodeOfAdvent
{
    public class DayTen : ICodingDay
    {
        public void Run(List<string> inputs)
        {
            bool printing = true;
            //       0        1   2    3        4   5
            // ex: position=< 7,  0> velocity=<-1,  0>
            var particles = new List<Particle>();

            foreach (var input in inputs)
            {
                particles.Add(new Particle(input));
            }

            while (printing)
            {
                PrintMap(particles);
                particles.ForEach(p => p.Move());
            }
        }

        private void PrintMap(List<Particle> particles)
        {
            StringBuilder builder = new StringBuilder();

            for (int row = 0; row < Location.HighY; row++)
            {
                var rowParticles = particles.Where(p => p.Location.Y == row).OrderBy(p => p.Location.X).ToList();
                if (rowParticles.Any())
                {
                    for (int col = 0; col < Location.HighX; col++)
                    {
                        if(rowParticles.Any())
                        {
                            var particle = rowParticles.FirstOrDefault(p => p.Location.X == col);
                            if (particle != null)
                            {
                                builder.Append('#');
                                rowParticles.Remove(particle);
                            }
                        }
                        else
                        {
                            builder.Append(' ');
                        }
                    }
                builder.AppendLine();
                }
                else
                {
                builder.AppendLine();

                }
            }

            Console.WriteLine(builder.ToString());
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