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

            Move move = new Move( this.Position, newPosition );

            if(this.ColorOfPieceAtNewPositionIsMyColor( board, newPosition ))
            {
                return false;
            }

            if(move.IsMoveDiagonal())
            {
                return board.IsPathToNewPositionClear( move );
            }

            return false;

        }

    }
}
