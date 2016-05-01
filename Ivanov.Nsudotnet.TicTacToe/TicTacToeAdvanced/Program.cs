using System;
using System.Collections.Generic;

namespace TicTacToeAdvanced
{
    class Program
    {
      public static void Main(string[] args)
        {
            GameController controller = new GameController();
            ConsoleGameInterface gameInterface = new ConsoleGameInterface(controller);
            controller.StartNewGame(PlayerType.X);
            controller.GameInterface = gameInterface;
            gameInterface.ListenConsole();
        }
    }
}