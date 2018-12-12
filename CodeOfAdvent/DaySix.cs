using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CodeOfAdvent
{
    public class DaySix : ICodingDay
    {
        public enum PointState
        {
            EmptyData,
            SharedData,
            ClaimedData,
        }

        public static int largestX = 0;
        public static int largestY = 0;
        public static int MaxDistance = 10000;

        public void Run(List<string> inputs)
        {
            PointState[][] Map;
            List<Location> PossibleLocations = new List<Location>();
            Char locationIDCounter = 'A';

            foreach (var input in inputs)
            {
                var parsed = input.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
                PossibleLocations.Add(new Location((locationIDCounter++).ToString(), Convert.ToInt32(parsed[0]), Convert.ToInt32(parsed[1])));
            }

            //populate map
            largestX = PossibleLocations.Max(p => p.Point.X)+1;
            largestY = PossibleLocations.Max(p => p.Point.Y)+1;

            Map = new PointState[largestX][];
            for (int index = 0; index < Map.Length; index++)
            {
                Map[index] = new PointState[largestY];
            }

            //Part1(PossibleLocations, Map);
            // part 2
            Part2(PossibleLocations, Map);
        }

        private void Part2(List<Location> possibleLocations, PointState[][] map)
        {
            int areaOfRegion = 0;
            for (int column = 0; column < map.Length; column++)
            {
                for (int row = 0; row < map[column].Length; row++)
                {
                    int distanceSum = 0;
                    foreach (var possibleLocation in possibleLocations)
                    {
                        distanceSum += possibleLocation.DistanceTo(column, row);
                        if (distanceSum > MaxDistance) break;
                    }

                    if (distanceSum < MaxDistance)
                    {
                        ++areaOfRegion;
                        Console.WriteLine($"Adding point {column},{row}");
                        map[column][row] = PointState.ClaimedData;
                    }
                }
            }

            Console.WriteLine($"Area is {areaOfRegion}");
        }

        public void Part1(List<Location> possibleLocations, PointState[][] map)
        {
            // expand territory of each location until no more points can be claimed.
            bool pointClaimed = false;
            int expansionCounter = 0;
            DateTime startTime = DateTime.Now;

            do
            {
                DateTime lastTime = DateTime.Now;
                pointClaimed = false;
                foreach (var possibleLocation in possibleLocations)
                {
                    pointClaimed |= possibleLocation.Expand(map);
                }
                foreach (var possibleLocation in possibleLocations)
                {
                    possibleLocation.Claim(map);
                }
                foreach (var possibleLocation in possibleLocations)
                {
                    possibleLocation.UpdateClaimCount(map);
                }
                ++expansionCounter;
                Console.WriteLine($"Expanded {expansionCounter} times after {(DateTime.Now - startTime).ToString("g")}. {(DateTime.Now - lastTime).TotalMilliseconds}ms passed this loop");
            } while (pointClaimed);

            // get infinite locations
            var infiniteLocations = possibleLocations.Where(loc => loc.IsInfinite).ToList();

            possibleLocations.RemoveAll(location => infiniteLocations.Contains(location));

            var largestLocation = possibleLocations.Aggregate((loc1, loc2) =>
            {
                return loc1.ClaimedCount > loc2.ClaimedCount ? loc1 : loc2;
            });
        }

        public class Location
        {
            public bool IsInfinite;
            public String ID;
            public int ClaimedCount = 0;
            public readonly Queue<Point> ClaimedPoints;
            public readonly Queue<Point> ExpandingPoints;
            public Point Point;

            public Location(string id, int x, int y)
            {
                ID = id;
                ClaimedPoints = new Queue<Point>();
                ExpandingPoints = new Queue<Point>();
                IsInfinite = false;
                Point = Point.GetPoint(x,y);
                ExpandingPoints.Enqueue(Point);
            }

            public bool Expand(PointState[][] map)
            {
                bool expanded = false;
                // expand to neighboring points
                while (ExpandingPoints.Any())
                {
                    var point = ExpandingPoints.Dequeue();
                    var mapValue = map[point.X][point.Y];
                    if ((mapValue == PointState.EmptyData || mapValue == PointState.SharedData) && !ClaimedPoints.Contains(point))
                    {
                        ClaimedPoints.Enqueue(point);
                        expanded = true;
                    }
                }

                return expanded;
            }

            public void Claim(PointState[][] map)
            {
                // claim all points in Claimed points and mark for expansion
                foreach (var claimedPoint in ClaimedPoints)
                {
                    if (map[claimedPoint.X][claimedPoint.Y] == PointState.EmptyData)
                    {
                        map[claimedPoint.X][claimedPoint.Y] = PointState.ClaimedData;
                    }
                    else if (map[claimedPoint.X][claimedPoint.Y] == PointState.ClaimedData)
                        map[claimedPoint.X][claimedPoint.Y] = PointState.SharedData;
                }

                foreach (var claimedPoint in ClaimedPoints)
                {
                    // check above
                    if (claimedPoint.Y > 0)
                    {
                        if (map[claimedPoint.X][claimedPoint.Y - 1] == PointState.EmptyData)
                        {
                            ExpandingPoints.Enqueue(Point.GetPoint(claimedPoint.X,claimedPoint.Y-1));
                        }
                    }

                    // check below
                    if (claimedPoint.Y < largestY -1)
                    {
                        if (map[claimedPoint.X][claimedPoint.Y + 1] == PointState.EmptyData)
                        {
                            ExpandingPoints.Enqueue(Point.GetPoint(claimedPoint.X, claimedPoint.Y + 1));
                        }
                    }

                    // check left
                    if (claimedPoint.X > 0)
                    {
                        if (map[claimedPoint.X-1][claimedPoint.Y] == PointState.EmptyData)
                        {
                            ExpandingPoints.Enqueue(Point.GetPoint(claimedPoint.X-1, claimedPoint.Y));
                        }
                    }

                    // check right
                    if (claimedPoint.X < largestX -1)
                    {
                        if (map[claimedPoint.X+1][claimedPoint.Y] == PointState.EmptyData)
                        {
                            ExpandingPoints.Enqueue(Point.GetPoint(claimedPoint.X+1, claimedPoint.Y));
                        }
                    }
                }

            }

            public void UpdateClaimCount(PointState[][] map)
            {
                // claim all points in Claimed points and mark for expansion
                foreach (var claimedPoint in ClaimedPoints)
                {
                    if (map[claimedPoint.X][claimedPoint.Y] == PointState.ClaimedData)
                    {
                        map[claimedPoint.X][claimedPoint.Y] = (PointState)ID.ToCharArray()[0];
                        ClaimedCount++;
                        IsInfinite |= claimedPoint.OnBoarder(largestX, largestY);
                    }
                }
                ClaimedPoints.Clear();
            }


            public override string ToString()
            {
                return $"Location {ID}:{Point}. Claimed: {ClaimedCount}. Expanding: {ExpandingPoints.Count}";
            }

            public int DistanceTo(int x, int y)
            {
                return Math.Abs(x - Point.X) + Math.Abs(y - Point.Y);
            }
        }

        public class Point
        {
            static List<Point> AllPoints = new List<Point>();

            // maybe use a pool for this.
            public static Point GetPoint(int x, int y)
            {
                var foundPoint = AllPoints.FirstOrDefault(p => p.X == x && p.Y == y);
                if (foundPoint == null)
                {
                    foundPoint = new Point(x,y);
                    AllPoints.Add(foundPoint);
                }
                return foundPoint;
            }

            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public bool OnBoarder(int largestX, int largestY)
            {
                return X == 0 || X >= largestX-1 || Y == 0 || Y >= largestY-1;
            }

            public override string ToString()
            {
                return $"({X},{Y})";
            }
        }
    }
}