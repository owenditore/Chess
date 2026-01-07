using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class King : Piece
    {

        public override bool CheckValidMove( Board board, Position newPosition )
        {

            Move move = new Move( this.Position, newPosition );

            if(this.ColorOfPieceAtNewPositionIsMyColor( board, newPosition ))
            {
                return false;
            }

            if(this.CheckIfNormalMoveIsValid( move ))
            {
                return true;
            }

            if(this.CheckIfMoveIsAValidCastleMove( move, board ))
            {
                return board.IsPathToNewPositionClear( move );
            }

            return false;

        }

        private bool CheckIfNormalMoveIsValid( Move move )
        {
            if(Math.Abs(move.Vertical) > 1)
                return false;

            if(Math.Abs( move.Horizontal ) > 1)
                return false;

            return true;
        }

        private bool CheckIfMoveIsAValidCastleMove( Move move, Board board )
        {
            if(this.HasMoved == true)
                return false;

            if(Math.Abs( move.Horizontal ) != 2)
                return false;

            if(move.Vertical != 0)
                return false;


            int targetRookColumn;

            if(move.Horizontal > 0)
            {
                targetRookColumn = 7;
            }
            else
            {
                targetRookColumn = 0;
            }

            Position targetRookPosition = new Position( this.Position.Row, targetRookColumn );

            Square? square = board.Squares.FirstOrDefault( s => s.Position.IsEqual( targetRookPosition ) );
            if(square.Piece == null)
            {
                return false;
            }

            if(square.Piece.IsARookThatCanCastle() == false)
            {
                return false;
            }

            return true;

        }

        public King( string name, string color, int row, int col ) : base( name, color, row, col )
        {

        }
    }
}
