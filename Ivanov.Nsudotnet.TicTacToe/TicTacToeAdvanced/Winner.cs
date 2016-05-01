using System;

namespace TicTacToeAdvanced
{
    public enum Winner
    {
        Unknown,
        X,
        O,
        Draw
    }

    static class WinnerExtension
    {
        public static FieldCell ConvertToFieldCell(this Winner winner)
        {
            switch (winner)
            {
                case Winner.Unknown:
                    return FieldCell.Empty;
                case Winner.X:
                    return FieldCell.X;
                case Winner.O:
                    return FieldCell.O;
                case Winner.Draw:
                    return FieldCell.Draw;
                default:
                    throw new ArgumentOutOfRangeException("winner", winner, null);
            }
        }
    }
}