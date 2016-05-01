using System;
using System.Linq;

namespace TicTacToeAdvanced
{
    public class GameCore
    {
        public PlayerType CurrentPlayer { get; private set; }
        private FieldCell[,] Field { get; set; }
        private FieldCell[,] SmallFieldWinners { get; set; }

        private bool lastTurnPositionSignificant = false;
        private int nextTurnSmallFieldX;
        private int nextTurnSmallFieldY;
        private IGameInterface gameInterface;

        public IGameInterface GameInterface
        {
            private get { return gameInterface; }
            set
            {
                gameInterface = value; 
                gameInterface.handleGameStateChanged(new GameState(Field, Winner.Unknown, CurrentPlayer));
            }
        }

        private static readonly int[][,] WinConditionLines =
        {
            //Rows
            new[,] {{0, 0}, {0, 1}, {0, 2}},
            new[,] {{1, 0}, {1, 1}, {1, 2}},
            new[,] {{2, 0}, {2, 1}, {2, 2}},

            //Columns
            new[,] {{0, 0}, {1, 0}, {2, 0}},
            new[,] {{0, 1}, {1, 1}, {2, 1}},
            new[,] {{0, 2}, {1, 2}, {2, 2}},

            //Diagonals
            new[,] {{0, 0}, {1, 1}, {2, 2}},
            new[,] {{0, 2}, {1, 1}, {2, 0}}
        };

        public GameCore(PlayerType startingPlayer)
        {
            Field = new FieldCell[9, 9];
            SmallFieldWinners = new FieldCell[3, 3];
            CurrentPlayer = startingPlayer;
        }

        private Winner ComputeFieldWinner(FieldCell[,] fieldCells, int xOffset, int yOffset)
        {
            foreach (var line in WinConditionLines)
            {
                if (ExtractFieldLine(fieldCells, xOffset, yOffset, line).All(cell => cell == FieldCell.O))
                {
                    return Winner.O;
                }
                if (ExtractFieldLine(fieldCells, xOffset, yOffset, line).All(cell => cell == FieldCell.X))
                {
                    return Winner.X;
                }
            }
            bool isDraw = WinConditionLines
                .Select(coordLine => ExtractFieldLine(fieldCells, xOffset, yOffset, coordLine).Distinct())
                .All(cells => 
                    (cells.Contains(FieldCell.X) ? 1 : 0) + 
                    (cells.Contains(FieldCell.O) ? 1 : 0) + 
                    (cells.Contains(FieldCell.Draw) ? 1 : 0) >= 2);
            return isDraw ? Winner.Draw : Winner.Unknown;
        }

        private FieldCell[] ExtractFieldLine(FieldCell[,] fieldCells, int x, int y, int[,] coordLine)
        {
            var cells = new FieldCell[3];
            for (int i = 0; i < 3; ++i)
            {
                cells[i] = fieldCells[coordLine[i, 0] + x, coordLine[i, 1] + y];
            }
            return cells;
        }

        public Winner MakeTurn(int x, int y)
        {
            if (Field[x, y] != FieldCell.Empty)
            {
                throw new GameException("Cell must be empty");
            }
            if (lastTurnPositionSignificant)
            {
                if (nextTurnSmallFieldX != x/3 || nextTurnSmallFieldY != y/3)
                {
                    var message = string.Format("Turn must be done in small field ({0}, {1}) of big field",
                        nextTurnSmallFieldX,
                        nextTurnSmallFieldY);
                    throw new GameException(message);
                }
            }
            Field[x, y] = CurrentPlayer == PlayerType.X ? FieldCell.O : FieldCell.X;
            var xSmallField = x/3;
            var ySmallField = y/3;
            var xSmallFieldOffset = xSmallField*3;
            var ySmallFieldOffset = ySmallField*3;

            lastTurnPositionSignificant = true;
            nextTurnSmallFieldX = x%3;
            nextTurnSmallFieldY = y%3;

            SmallFieldWinners[xSmallField, ySmallField] = ComputeFieldWinner(Field, xSmallFieldOffset, ySmallFieldOffset)
                .ConvertToFieldCell();
            var winner = ComputeFieldWinner(SmallFieldWinners, 0, 0);
            CurrentPlayer = CurrentPlayer == PlayerType.X ? PlayerType.O : PlayerType.X;

            if (GameInterface != null)
            {
                GameInterface.handleGameStateChanged(new GameState(Field, winner, CurrentPlayer));
            }
            return winner;
        }
    }
}