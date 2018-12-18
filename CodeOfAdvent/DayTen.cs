using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;

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
            
            string lastBestDisplay = "";
            int currentHeight= Int32.MaxValue;
            int lastHeight = currentHeight-1;

            // find ballpark of number
            //while (lastHeight < currentHeight)
            //{
            //    currentHeight = lastHeight;
            //    ++loopCount;
            //    particles.ForEach(p => p.Move());
            //    UpdateBounds(particles);

            //    lastHeight = GetHeight();
            //}

            int loopCount = 0;

            for (int looper = 0; looper < 10000; looper++, loopCount++)
            {
                particles.ForEach(p => p.Move());
            }

            while (true)
            {
                // cycle though and check manually.
                particles.ForEach(p => p.Move());
                UpdateBounds(particles);

                lastBestDisplay = PrintMap(particles.OrderBy(p => p.Location.X).ThenBy(p => p.Location.Y).ToList());
                DisplayGuess(lastBestDisplay);
                loopCount++;
            }
        }

        private int GetHeight()
        {
            return Math.Abs(Location.LowY - Location.HighY);
        }

        private void DisplayGuess(string lastBestDisplay)
        {
            Console.WriteLine(lastBestDisplay);
            File.WriteAllText("OutputDay10.txt", lastBestDisplay);
        }

        private void UpdateBounds(List<Particle> particles)
        {
            Location.LowY = particles.Min(p => p.Location.YLocation);
            Location.LowX = particles.Min(p => p.Location.XLocation);
            Location.HighX = int.MinValue;
            Location.HighY = int.MinValue;

            particles.ForEach(p => p.Location.UpdateBounds());
        }

        protected int GetGroupCount(List<Particle> particles)
        {
            int groupCount = 0;
            var uncheckedParticles = particles.ToList();

            while(uncheckedParticles.Any())
            {
                ++groupCount;

                var currentParticles = uncheckedParticles.GetRange(0,1);

                foreach (var currentParticle in currentParticles)
                {
                    uncheckedParticles.Remove(currentParticle);
                }
                while(currentParticles.Any())
                {
                    var adjacentParticles = new List<Particle>();
                    foreach (var p in currentParticles)
                    {
                        var adjacent = p.NextTo(uncheckedParticles);
                        foreach (var particle in adjacent)
                        {
                            uncheckedParticles.Remove(particle);
                        }
                        adjacentParticles.AddRange(adjacent);
                    }
                    currentParticles = adjacentParticles;
                }
            }

            return groupCount;
        }

        private string PrintMap(List<Particle> particles)
        {
            StringBuilder builder = new StringBuilder();

            var map = new char[Location.HighY + 1][];
            //clear the map
            for (int index = 0; index < map.Length; index++)
            {
                map[index] = Enumerable.Repeat(' ', Location.HighX+1).ToArray();
            }

            //fill with particles
            foreach (var particle in particles)
            {
                map[particle.Location.Y][particle.Location.X] = '#';
            }

            foreach (var row in map)
            {
                builder.Append(row);
                builder.AppendLine();
            }

            return builder.ToString();
        }


        protected class Particle
        {
            public Location Location;
            public Velocity Velocity;

            public Particle(string input)
            {
                var parse = input.Split(new []{ ',', '<', '>' }, StringSplitOptions.RemoveEmptyEntries);
                Location = new Location(Convert.ToInt32(parse[1]), Convert.ToInt32(parse[2]));
                Velocity = new Velocity(Convert.ToInt32(parse[4]), Convert.ToInt32(parse[5]));
            }

            public void Move()
            {
                Location = Location + Velocity;
            }
            public void MoveBack()
            {
                Location = Location - Velocity;
            }
            public override string ToString()
            {
                return $"P({Location}. {Velocity}";
            }

            public List<Particle> NextTo(List<Particle> otherParticles)
            {
                return otherParticles.Where(o => Location.NextTo(o.Location)).ToList();
            }
        }

        protected class Location
        {
            public int XLocation;
            public int YLocation;
            public static int HighX;
            public static int LowX;
            public static int HighY;
            public static int LowY;

            public Location(int x, int y)
            {
                X = x;
                Y = y;

                LowX = Math.Min(x, LowX);
                LowY = Math.Min(y, LowY);
                HighX = Math.Max(X, HighX);
                HighY = Math.Max(Y, HighY);
            }

            public virtual int X
            {
                get
                {
                    return XLocation - LowX;
                }
                set { XLocation = value; }
            }

            public virtual int Y
            {
                get { return YLocation - LowY; }
                set { YLocation = value; }
            }

            public override string ToString()
            {
                return $"<{X},{Y}>";
            }

            public static Location operator +(Location l1, Location l2)
            {
                return new Location(l1.XLocation + l2.XLocation, l1.YLocation + l2.YLocation);
            }
            public static Location operator -(Location l1, Location l2)
            {
                return new Location(l1.XLocation - l2.XLocation, l1.YLocation - l2.YLocation);
            }

            public void UpdateBounds()
            {
                HighX = Math.Max(X, HighX);
                HighY = Math.Max(Y, HighY);
            }

            public bool NextTo(Location otherLocation)
            {
                return (XLocation == otherLocation.XLocation && Math.Abs(YLocation - otherLocation.YLocation) <= 1) ||
                       (YLocation    == otherLocation.YLocation && Math.Abs(XLocation - otherLocation.XLocation) <= 1);
            }
        }

        protected class Velocity : Location
        {
            public Velocity(int x, int y) : base(x, y)
            {
                XLocation = x;
                YLocation = y;
            }

            public override int X { get { return XLocation; } set { XLocation = value; } }
            public override int Y { get { return YLocation; } set { YLocation = value; } }
        }
    }

}