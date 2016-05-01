using System;
using System.Text;

namespace TicTacToeAdvanced
{
    public class ConsoleGameInterface : IGameInterface
    {
        private bool gameOver = false;
        private GameController controller;

        public ConsoleGameInterface(GameController controller)
        {
            this.controller = controller;
        }

        public void ListenConsole()
        {
            while (true)
            {
                if (HandleGameOver()) break;
                Console.Out.WriteLine("Enter coordinates of target:");
                while (true)
                {
                    try
                    {
                        var values = Console.In.ReadLine().Split(' ');
                        if (values.Length != 2)
                        {
                            Console.Out.WriteLine("Format: x y");
                            continue;
                        }
                        var x = int.Parse(values[0]);
                        var y = int.Parse(values[1]);

                        controller.MakeTurn(x, y);
                        break;
                    }
                    catch (GameException e)
                    {
                        Console.Out.WriteLine(e.Message);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.Out.WriteLine("Coordinates must be between 0 and 8");
                    }
                    catch (FormatException e)
                    {
                        Console.Out.WriteLine("Coordinates must be integer");
                    }
                }
            }
        }

        private bool HandleGameOver()
        {
            if (gameOver)
            {
                Console.Out.WriteLine("Play again? (y/n)");
                var line = Console.In.ReadLine();
                if (line == "y")
                {
                    controller.StartNewGame(PlayerType.X);
                    gameOver = false;
                    Console.Clear();
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public void handleGameStateChanged(GameState state)
        {
            Console.Clear();
            Console.Out.WriteLine("Winner: " + state.Winner);
            Console.Out.WriteLine("╔═══╦═══╦═══╗");
            for (int y = 0; y < 9; y++)
            {
                if (y%3 == 0 && y != 0)
                {
                    Console.Out.WriteLine("╠═══╬═══╬═══╣");
                }
                StringBuilder row = new StringBuilder();
                for (int x = 0; x < 9; ++x)
                {
                    if (x%3 == 0)
                    {
                        row.Append("║");
                    }
                    row.Append(state.Field[x, y].ToChar());
                }
                row.Append("║");
                Console.Out.WriteLine(row);
            }
            Console.Out.WriteLine("╚═══╩═══╩═══╝");
            if (state.Winner == Winner.Unknown)
            {
                Console.Out.WriteLine("Next player: " + state.NextTurnPlayer);
            }
            if (state.Winner != Winner.Unknown)
            {
                gameOver = true;
            }
        }
    }
}