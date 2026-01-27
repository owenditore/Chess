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

            if(board.CheckForPiece( move.EndingPosition ) != "none")
                return false;


            int targetRookColumn = FindTargetRookColumn( move );

            Position targetRookPosition = new Position( this.Position.Row, targetRookColumn );

            Piece? rook = board.Pieces.FirstOrDefault( r => r.IsARookThatCanCastle( targetRookPosition ) );

            if(rook == null)
                return false;

            return true;

        }

        private int FindTargetRookColumn(Move move)
        {
            if(move.Horizontal > 0)
            {
                return 7;
            }
            else
            {
                return 0;
            }
        }

        public King( string name, string color, int row, int col ) : base( name, color, row, col )
        {

        }
    }
}
