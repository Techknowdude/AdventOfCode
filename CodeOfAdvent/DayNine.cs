using System;
using System.Collections.Generic;

namespace CodeOfAdvent
{
    public class DayNine : ICodingDay
    {
        public void Run(List<string> inputs)
        {
            //428 players; last marble is worth 72061 points
            List<int> board = new List<int>();
            int currentLocation = 0;
            Queue<Player> players = new Queue<Player>();

            var playerCount = Convert.ToInt32(inputs[0]);
            var highScore = Convert.ToInt32(inputs[6]);
            var highestMarbleWon = 0;

            //create players
            for (int playerNumber = 0; playerNumber < playerCount; playerNumber++)
            {
                players.Enqueue(new Player() { ID = playerNumber});
            }
            Player player = null;
            int playCounter = 0;
            while(highestMarbleWon != highScore)
            {
                player = players.Dequeue();

                if (playCounter%23 == 0 && playCounter > 0)
                {
                    // add score
                    var marble = board[currentLocation];
                    highestMarbleWon = highestMarbleWon > marble ? highestMarbleWon : marble;

                    var previousMarble = board[WrapLocation(currentLocation - 7, board.Count)];

                    board.Remove(previousMarble);
                    // adjust for removing a marble before the current marble.
                    currentLocation = board.IndexOf(marble);
                    board.Remove(marble);

                    player.Score += marble + previousMarble;

                    currentLocation = WrapLocation(currentLocation - 1, board.Count);
                }
                else
                {
                    // add new marble
                    board.Insert(currentLocation, playCounter);
                }

                players.Enqueue(player);
                playCounter++;
            }

            Console.WriteLine(player.Score);
        }

        private int WrapLocation(int location, int max)
        {
            return location%max;
        }

        class Player
        {
            public int ID;
            public int Score;
        }

        public int TotalPlayCount = 25;
    }
}