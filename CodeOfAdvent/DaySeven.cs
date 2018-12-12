using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeOfAdvent
{
    public class DaySeven : ICodingDay
    {
        public void Run(List<string> inputs)
        {
            // link all steps
            foreach (var input in inputs)
            {
                var parse = input.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                var parent = Link.GetLink(parse[1][0]);
                var child = Link.GetLink(parse[7][0]);

                parent.ChildrenLinks.Add(child);
                child.ParentLinks.Add(parent);
            }

            //PartOne();

            PartTwo();
        }

        private void PartTwo()
        {
            List<Link> firstLinks = null;

            // get last link
            firstLinks = Link.AllLinks.Where(l => !l.ParentLinks.Any()).ToList();

            var order = GetAssemblyOrderPartTwo(firstLinks);

            Console.WriteLine(order);
        }

        public string GetAssemblyOrderPartTwo(List<Link> rootLinks)
        {
            string result = "";
            int time = 0;

            List<Link> AvailableLinks = new List<Link>(rootLinks);
            List<Link> CompletedLinks = new List<Link>();
            List<Worker> Elves = new List<Worker>() { new Worker("Padraic"), new Worker("Brandon"), new Worker("Jess"), new Worker("Jayden"), new Worker("Aaron"), };

            while (AvailableLinks.Any() || Elves.Any(elf => elf.Working))
            {
                ++time;
                AvailableLinks = AvailableLinks.OrderBy(link => link.Name).ToList();

                FillWorkers(AvailableLinks, Elves);

                var finishedWork = DoWork(Elves);

                foreach (var finishedJob in finishedWork.OrderBy(l => l.Name))
                {
                    CompletedLinks.Add(finishedJob);
                    result += finishedJob.Name;
                    // add links with all parents completed
                    AvailableLinks.AddRange(finishedJob.ChildrenLinks.Where(c => c.ParentLinks.All(p => CompletedLinks.Contains(p))));
                }
            }

            Console.WriteLine(time);

            return result;
        }

        private void FillWorkers(List<Link> availableLinks, List<Worker> elves)
        {
            foreach (var worker in elves)
            {
                if (!worker.Working && availableLinks.Any())
                {
                    worker.StartJob(availableLinks[0]);
                    availableLinks.RemoveAt(0);
                }
            }
        }

        private List<Link> DoWork(List<Worker> elves)
        {
            List<Link> finishedJobs = new List<Link>();

            foreach (var worker in elves)
            {
                var job = worker.DoWork();
                if(job != null)
                    finishedJobs.Add(job);
            }

            return finishedJobs;
        }

        public class Worker
        {
            private Link Job;
            private int timeWorked = 0;
            public bool Working = false;
            private string name;

            public Worker(string name)
            {
                this.name = name;
            }

            public void StartJob(Link link)
            {
                Job = link;
                timeWorked = 0;
                Working = true;
            }

            public Link DoWork()
            {
                if (!Working) return null;

                ++timeWorked;
                if (timeWorked >= Job.GetTimeToWork())
                {
                    Working = false;
                    return Job;
                }
                return null;
            }

            public int WorkLeft { get { return Working ? Job.GetTimeToWork() - timeWorked : 0; } }

            public override string ToString()
            {
                return $"Worker {name} working on {Job.Name}. {WorkLeft} remaining";
            }
        }

        private void PartOne()
        {
            List<Link> firstLinks = null;

            // get last link
            firstLinks = Link.AllLinks.Where(l => !l.ParentLinks.Any()).ToList();

            var order = GetAssemblyOrderPartOne(firstLinks);

            Console.WriteLine(order);
        }

        public string GetAssemblyOrderPartOne(List<Link> rootLinks )
        {
            string result = "";

            List<Link> AvailableLinks = new List<Link>(rootLinks);
            List<Link> CompletedLinks = new List<Link>();

            while (AvailableLinks.Any())
            {
                AvailableLinks = AvailableLinks.OrderBy(link => link.Name).ToList();
                var bestLink = AvailableLinks[0];
                AvailableLinks.RemoveAt(0);
                CompletedLinks.Add(bestLink);
                result += bestLink.Name;
                // add links with all parents completed
                AvailableLinks.AddRange(bestLink.ChildrenLinks.Where(c => c.ParentLinks.All(p => CompletedLinks.Contains(p))));
            }

            return result;
        }

        public class Link
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

            public int GetTimeToWork()
            {
                return 60 + (Name - 'A') + 1;
            }

        }
    }
}