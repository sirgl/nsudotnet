using System;

namespace TicTacToeAdvanced
{
    public enum FieldCell
    {
        Empty,
        X,
        O,
        Draw //occupied not by x or o
    }

    static class FieldCellExtension
    {
        public static char ToChar(this FieldCell cell)
        {
            switch (cell)
            {
                case FieldCell.Empty:
                    return ' ';
                case FieldCell.X:
                    return 'X';
                case FieldCell.O:
                    return 'O';
                case FieldCell.Draw:
                    return '?';
                default:
                    throw new ArgumentOutOfRangeException("cell", cell, null);
            }
        }
    }
}