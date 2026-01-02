using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Rook : Piece
    {

        //Constructor
        public Rook( string name, string color, int row, int col ) : base( name, color, row, col )
        {

        }

        //Methods

        public override bool CheckValidMove( Board board, Position newPosition )
        {

            Move move = new Move( this.Position, newPosition );

            if(this.ColorOfPieceAtNewPositionIsMyColor( board, newPosition ))
            {
                return false;
            }

            if(this.MoveFollowsRookMovementRules( move ))
            {
                return board.IsPathToNewPositionClear( move );
            }

            return false;

        }

        private bool MoveFollowsRookMovementRules( Move move )
        {
            if(move.IsMoveVertical()) return true;

            if(move.IsMoveHorizontal()) return true;

            return false;
        }

    }
}
