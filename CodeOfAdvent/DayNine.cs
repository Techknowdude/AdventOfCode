using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeOfAdvent
{
    public class DayNine : ICodingDay
    {
        public void Run(List<string> inputs)
        {
            inputs = inputs[0].Split(new char[] { ' ' }).ToList();
            //428 players; last marble is worth 72061 points
            CircularLinkedList<int> board = new CircularLinkedList<int>() {};
            var currentNode = board.Add(0);

            int currentLocation = 0;
            Queue<Player> players = new Queue<Player>();

            var playerCount = Convert.ToInt32(inputs[0]);
            var highScore = Convert.ToInt32(inputs[6]) * 100;

            //create players
            for (int playerNumber = 0; playerNumber < playerCount; playerNumber++)
            {
                players.Enqueue(new Player() { ID = playerNumber});
            }
            Player player = null;
            int playCounter = 1;
            do
            {
                player = players.Dequeue();

                if (playCounter%23 == 0 && playCounter > 0)
                {
                    // move marble back seven
                    currentNode = board.GetNode(currentNode, -7);
                    var previousMarble = currentNode.Value;

                    currentNode = board.Remove(currentNode);

                    // add score
                    player.Score += playCounter + previousMarble;
                    //Console.WriteLine($"{highScore - playCounter} to go. Awarding {playCounter}");
                }
                else
                {
                    // add new marble
                    if (currentNode.Next != null)
                    {
                        currentNode = board.AddAfter(currentNode.Next, playCounter);
                    }
                    else
                    {
                        currentNode = board.AddAfter(currentNode, playCounter);
                    }
                }

                players.Enqueue(player);
                playCounter++;
            } while (playCounter < highScore);

            var winningPlayer = players.Aggregate((p1, p2) => { return p1.Score > p2.Score ? p1 : p2; });

            Console.WriteLine(winningPlayer.Score);
        }


        class Player
        {
            public int ID;
            public double Score;
        }

        public int TotalPlayCount = 25;
    }

    class LinkNode<T>
    {
        public LinkNode<T> Next;
        public LinkNode<T> Previous;
        public T Value;

        public LinkNode(T val)
        {
            Value = val;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    class CircularLinkedList<T>
    {
        private LinkNode<T> CurrentNode = null;
        
        public LinkNode<T> AddAfter(LinkNode<T> node, T val, int offset = 0)
        {
            var newNode = new LinkNode<T>(val);
            if (CurrentNode == null)
            {
                CurrentNode = newNode;
                CurrentNode.Next = CurrentNode;
                CurrentNode.Previous = CurrentNode;
            }
            else
            {
                while (offset > 0)
                {
                    node = node.Next;
                    --offset;
                }
                while (offset < 0)
                {
                    node = node.Previous;
                    ++offset;
                }
            }

            var nextNode = node.Next;
            node.Next = newNode;
            newNode.Next = nextNode;
            newNode.Previous = node;
            nextNode.Previous = newNode;

            CurrentNode = newNode;

            return newNode;
        }

        public LinkNode<T> Remove(LinkNode<T> node)
        {
            if (node == CurrentNode)
            {
                CurrentNode = node.Next;
            }

            var prev = node.Previous;
            var next = node.Next;
            prev.Next = next;
            next.Previous = prev;

            return CurrentNode;
        }

        public LinkNode<T> Add(T val)
        {
            if (CurrentNode == null)
            {
                CurrentNode = new LinkNode<T>(val);
                CurrentNode.Previous = CurrentNode;
                CurrentNode.Next = CurrentNode;
                return CurrentNode;
            }

            return AddAfter(CurrentNode, val);
        }

        internal LinkNode<T> GetNode(LinkNode<T> currentNode, int offset)
        {
            while (offset < 0)
            {
                currentNode = currentNode.Previous;
                ++offset;
            }
            while(offset > 0)
            {
                currentNode = currentNode.Next;
                --offset;
            }
            CurrentNode = currentNode;
            return CurrentNode;
        }

        public override string ToString()
        {
            string output = String.Empty;

            var start = CurrentNode;

            do
            {
                output += start.ToString();
            }while ((start = start.Next) != CurrentNode);

                return output;
        }
    }
}