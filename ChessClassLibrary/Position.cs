using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position(int row, int column)
        {
            Row = row; 
            Column = column; 
        }

        public string SquareColor()
        {
            if (((Row + Column) % 2) == 0)
                return "light";
            else
                return "dark";
        }

        public bool IsEqual(Position otherPosition)
        {
            int comparedRow = otherPosition.Row;
            int comparedColumn = otherPosition.Column;

            if(this.Row == comparedRow && this.Column == comparedColumn)
            {
                return true;
            }

            return false;
        }
    }
}
