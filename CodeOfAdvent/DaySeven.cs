using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeOfAdvent
{
    public class DaySeven : ICodingDay
    {
        public void Run(List<string> inputs)
        {
            Link firstLink = null;

            // Step X must be finished before step I can begin.
            foreach (var input in inputs)
            {
                var parse = input.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                var parent = Link.GetLink(parse[1][0]);
                var child = Link.GetLink(parse[7][0]);

                parent.ChildrenLinks.Add(child);
                child.ParentLinks.Add(parent);
            }

            // get last link
            firstLink = Link.AllLinks.FirstOrDefault(l => !l.ParentLinks.Any());

            var order = firstLink.GetAssemblyOrder();

            Console.WriteLine(order);
        }

        class Link
        {
            public static List<Link> AllLinks = new List<Link>();

            public static Link GetLink(char name)
            {
                var link = AllLinks.FirstOrDefault(l => l.Name == name);
                if (link == null)
                {
                    link = new Link(name);
                    AllLinks.Add(link);
                }
                return link;
            }

            public List<Link> ChildrenLinks;
            public List<Link> ParentLinks;
            public char Name;

            public Link(char name)
            {
                this.Name = name;
                ChildrenLinks = new List<Link>();
                ParentLinks = new List<Link>();
            }

            public string GetChildNames()
            {
                StringBuilder str = new StringBuilder("(");

                foreach (var childrenLink in ChildrenLinks)
                {
                    str.Append($"{childrenLink.Name},");
                }

                return str.Append(")").ToString();
            }

            public override string ToString()
            {
                return $"Link {Name}: ({GetChildNames()}";
            }

            public string GetAssemblyOrder()
            {
                string result = "";

                List<Link> AvailableLinks = new List<Link>();

                AvailableLinks.Add(this);

                while (AvailableLinks.Any())
                {
                    AvailableLinks = AvailableLinks.OrderBy(link => link.Name).ToList();
                    var bestLink = AvailableLinks[0];
                    AllLinks.ForEach(l => l.p);
                    AvailableLinks.RemoveAt(0);
                    result = bestLink.Name + result;
                    AvailableLinks.AddRange(bestLink.ParentLinks);
                }

                return result;
            }
        }
    }
}