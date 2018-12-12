using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeOfAdvent
{
    public class DayEight : ICodingDay
    {
        public void Run(List<string> inputs)
        {
            inputs = inputs[0].Split(' ').ToList();
            Node rootNode = new Node();
            rootNode.Initialize(inputs);

            // part1
            Console.WriteLine(rootNode.GetMetaTotal);

            // part2
            Console.WriteLine(rootNode.GetMetaPartTwo);
        }
        //n1  n2  m2 m2 m2 n3  n4  m4 m3 m1 m1 m1
        //2 3 0 3 10 11 12 1 1 0 1 99 2  1  1  2
        class Node
        {
            List<Node> Children = new List<Node>();
            List<int> MetaData = new List<int>();
            private static char names = 'A';
            public char Name = names++;

            public Node()
            {
            }

            public List<string> Initialize(List<string> input)
            {
                var childrenCount = Convert.ToInt32(input[0]);
                var metaCount = Convert.ToInt32(input[1]);

                // create children
                for (int childIndex = 0; childIndex < childrenCount; childIndex++)
                {
                    Node child = new Node();
                    Children.Add(child);
                    input = child.Initialize(input.GetRange(2,input.Count-2));
                }

                // get meta data
                var metas = input.GetRange(2, metaCount);
                MetaData.AddRange(metas.Select(m => Convert.ToInt32(m)));
                // remove meta from list

                return input.GetRange(metaCount,input.Count-metaCount);
            }
            
            public int GetMetaTotal
            {
                get
                {
                    return MetaData.Sum() + (Children?.Sum(c => c.GetMetaTotal) ?? 0);
                }
            }

            public int GetMetaPartTwo
            {
                get {
                    if(!Children.Any())
                        return  MetaData.Sum();

                    var total = 0;
                    foreach (var metaIndex in MetaData)
                    {
                        if(Children.Count >= metaIndex)
                            total += Children[metaIndex-1].GetMetaPartTwo;
                    }
                    return total;
                }
            }

            public override string ToString()
            {
                return $"Node {Name}. ({GetChildNames()})";
            }

            private string GetChildNames()
            {
                string children = "";
                foreach (var child in Children)
                {
                    children += child.ToString();
                }
                return children;
            }
        }
    }
}