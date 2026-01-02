using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Move
    {

        public int Vertical { get; set; }

        public int Horizontal { get; set; }


        public Position StartingPosition { get; set; }

        public Position EndingPosition { get; set; }

        public Move( Position startingPosition, Position endingPosition )
        {
            this.StartingPosition = startingPosition;
            this.EndingPosition = endingPosition;
            this.Vertical = endingPosition.Row - startingPosition.Row;
            this.Horizontal = endingPosition.Column - startingPosition.Column;
        }

        public string IsMoveVerticalHorizontalOrDiagonal()
        {
            if(this.IsMoveVertical()) return "vertical";

            if(this.IsMoveHorizontal()) return "horizontal";

            if(this.IsMoveDiagonal()) return "diagonal";

            return "none";
        }

        public bool IsMoveVertical()
        {
            if(this.Vertical != 0 && this.Horizontal == 0) return true;
            return false;
        }
        public bool IsMoveHorizontal()
        {
            if(this.Vertical == 0 && this.Horizontal != 0) return true;
            return false;
        }
        public bool IsMoveDiagonal()
        {
            if(Math.Abs(this.Vertical) == Math.Abs(this.Horizontal) && this.Vertical != 0) return true;
            return false;
        }

        public int DistanceCloserToZero( int number )
        {
            if(number > 0)
            {
                return number - 1;
            }
            else if(number < 0)
            {
                return number + 1;
            }
            else
                return number;
        }

    }
}
