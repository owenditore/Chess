using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Queen : Piece
    {

        public override bool CheckValidMove( Board board, Position newPosition )
        {

            Move move = new Move( this.Position, newPosition );

            if(this.ColorOfPieceAtNewPositionIsMyColor( board, newPosition ))
            {
                return false;
            }

            if(this.MoveFollowsQueenMovementRules( move ))
            {
                return board.IsPathToNewPositionClear( move );
            }


            return false;

        }

        private bool MoveFollowsQueenMovementRules( Move move )
        {

            if(move.IsMoveVertical()) return true;

            else if(move.IsMoveHorizontal()) return true;

            else if(move.IsMoveDiagonal()) return true;

            else return false;

        }

        public Queen(string name, string color, int row, int col) : base(name, color, row, col)
        {

        }

    }
}
