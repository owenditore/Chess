using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Bishop : Piece
    {

        //Constructor
        public Bishop(string name, string color, int row, int col) : base(name, color, row, col)
        {

        }

        //Methods

        public override bool CheckValidMove(Board board, Position newPosition)
        {
            Position intermediaryPosition = new Position(-1, -1);
            string stateOfNewPosition = board.CheckForPiece(newPosition); //white,black,none
            int verticalMove = newPosition.Row - Position.Row;
            int horizontalMove = newPosition.Column - Position.Column;

            //No friendly piece at new position
            if (stateOfNewPosition == Color)
            {
                return false;
            }

            //Moves diagonally
            else if (Math.Abs(verticalMove) == Math.Abs(horizontalMove) && verticalMove != 0)
            {
                if (Math.Abs(verticalMove) == 1)
                {

                    return true;
                }
                do
                {
                    verticalMove = MoveCloserToZero(verticalMove);
                    horizontalMove = MoveCloserToZero(horizontalMove);
                    intermediaryPosition.Row = verticalMove + Position.Row;
                    intermediaryPosition.Column = horizontalMove + Position.Column;
                    string stateOfIntermediaryPosition = board.CheckForPiece(intermediaryPosition);
                    if (stateOfIntermediaryPosition != "none")
                    {
                        return false;
                    }

                } while (Math.Abs(verticalMove) != 1);


                return true;
            }

            else
            {
                return false;
            }
        }

    }
}
