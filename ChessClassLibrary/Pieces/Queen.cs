using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Queen : Piece
    {

        //Constructor
        public Queen(string name, string color, int row, int col) : base(name, color, row, col)
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

            //Moves vertically but not horizontally
            else if (verticalMove != 0 && horizontalMove == 0 && CheckIfPieceCanMoveVertically( verticalMove, board, intermediaryPosition ))
            {
                return true;
            }

            //Moves horizontally but not vertically
            else if (verticalMove == 0 && horizontalMove != 0 && CheckIfPieceCanMoveHorizontally( horizontalMove, board, intermediaryPosition ))
            {
                return true;
            }

            //Moves diagonally
            else if (Math.Abs(verticalMove) == Math.Abs(horizontalMove) && verticalMove != 0 && CheckIfPieceCanMoveDiagonally( verticalMove, horizontalMove, board, intermediaryPosition ))
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
