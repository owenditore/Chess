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


            if (stateOfNewPosition == Color)
            {
                return false;
            }

            else if(Math.Abs( verticalMove ) != Math.Abs( horizontalMove ) || verticalMove == 0)
            {
                return false;
            }

            else if(this.CheckIfMoveDoesNotHitIntermediatePiecesDiagonally( verticalMove, horizontalMove, board, intermediaryPosition ))
            {
                return true;
            }

            else
            {
                return false;
            }
        }

    }
}
