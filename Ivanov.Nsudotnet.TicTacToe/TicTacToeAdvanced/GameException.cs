using System;

namespace TicTacToeAdvanced
{
    public class GameException : Exception
    {
        public GameException(string message) : base(message)
        {
        }
    }
}