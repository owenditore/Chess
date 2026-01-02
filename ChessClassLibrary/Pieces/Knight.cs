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

            Move move = new Move( this.Position, newPosition );

            if(this.ColorOfPieceAtNewPositionIsMyColor( board, newPosition ))
            {
                return false;
            }

            if(this.MoveFollowsKnightMovementRules( move ))
            {
                return true;
            }

            return false;

        }

        private bool MoveFollowsKnightMovementRules( Move move )
        {
            if(Math.Abs( move.Vertical ) == 2 && Math.Abs( move.Horizontal ) == 1) return true;

            if(Math.Abs( move.Vertical ) == 1 && Math.Abs( move.Horizontal ) == 2) return true;

            return false;
        }
    }
}
