using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Knight : Piece
    {

        //Constructor
        public Knight(string name, string color, int row, int col) : base(name, color, row, col)
        {

        }

        //Methods

        public override bool CheckValidMove(Board board, Position newPosition)
        {
            string stateOfNewPosition = board.CheckForPiece(newPosition); //white,black,none
            int verticalMove = newPosition.Row - Position.Row;
            int horizontalMove = newPosition.Column - Position.Column;

            //No friendly piece at new position
            if (stateOfNewPosition == Color)
            {
                return false;
            }

            //2 Vertically and 1 Horizontally
            else if (Math.Abs(verticalMove) == 2 && Math.Abs(horizontalMove) == 1)
            {
                return true;
            }

            //2 Horizontally and 1 Vertically
            else if (Math.Abs(verticalMove) == 1 && Math.Abs(horizontalMove) == 2)
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
