using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessClassLibrary
{
    public class Pawn : Piece
    {

        //Constructor
        public Pawn( string name, string color, int row, int col ) : base( name, color, row, col )
        {

        }

        //Methods

        public override bool NeedToPromote()
        {
            if(this.Position.Row == 0)
                return true;
            if(this.Position.Row == 7)
                return true;

            return false;
        }
        private bool CheckIfMoveIsForward( Move move )
        {
            switch(this.Color)
            {
                case "white":
                    if(move.Vertical < 0)
                        return true;
                    break;

                case "black":
                    if(move.Vertical > 0)
                        return true;
                    break;
            }

            return false;
        }

        private bool CheckForValidNormalMove( Move move, string stateOfNewPosition )
        {
            if(Math.Abs( move.Vertical ) == 1 && move.Horizontal == 0 && stateOfNewPosition == "none")
                return true;

            else return false;

        }

        private bool CheckForValidFirstMove( Move move, string stateOfNewPosition, Board board )
        {
            if(Math.Abs( move.Vertical ) != 2) return false;

            if(move.Horizontal != 0) return false;

            if(stateOfNewPosition != "none") return false;

            if(this.HasMoved == true) return false;

            return board.IsPathToNewPositionClear( move );

        }

        private bool CheckForValidCapture( Move move, string stateOfNewPosition )
        {
            if(move.IsMoveDiagonal() == false) return false;

            if(Math.Abs( move.Vertical ) != 1) return false;

            if(stateOfNewPosition != this.OpponentColor) return false;

            return true;

        }

        private bool CheckForValidEnPassantMove( Move move, string stateOfNewPosition, Board board )
        {
            if(move.IsMoveDiagonal() == false) return false;

            if(Math.Abs( move.Vertical ) != 1) return false;

            if(stateOfNewPosition != "none") return false;

            Position enPassantPieceToCaptureCurrentPosition = new Position( FindEnPassantCurrentRow( move ), move.EndingPosition.Column );
            Position enPassantPieceToCapturePriorPosition = new Position( FindEnPassantStartRow( enPassantPieceToCaptureCurrentPosition ), move.EndingPosition.Column );

            Turn lastTurn = board.ReturnLastTurn( board );

            if(lastTurn == null) return false;

            return CheckForValidOpponentPawnAtEnPassantPosition( enPassantPieceToCaptureCurrentPosition, enPassantPieceToCapturePriorPosition, lastTurn, board );

        }

        private int FindEnPassantCurrentRow( Move move )
        {
            switch(this.Color)
            {
                case "white":
                    return move.EndingPosition.Row + 1;

                case "black":
                    return move.EndingPosition.Row - 1;

            }
            return -1;
        }

        private int FindEnPassantStartRow( Position currentPosition )
        {
            switch(this.Color)
            {
                case "white":
                    return currentPosition.Row - 2;

                case "black":
                    return currentPosition.Row + 2;

            }
            return -1;
        }


        private bool CheckForValidOpponentPawnAtEnPassantPosition( Position toCaptureCurrentPosition, Position toCapturePriorPosition, Turn lastTurn, Board board )
        {

            if(lastTurn.Piece.Name != "pawn")
                return false;

            if(lastTurn.StartingPosition.IsEqual( toCapturePriorPosition ) != true)
                return false;

            if(lastTurn.EndingPosition.IsEqual( toCaptureCurrentPosition ) != true)
                return false;

            return true;

        }

        private bool CheckIfAnyPawnMoveTypeIsValid( Move move, string stateOfNewPosition, Board board )
        {
            if(this.CheckIfMoveIsForward( move ) == false)
                return false;

            if(this.CheckForValidNormalMove( move, stateOfNewPosition ))
                return true;

            if(this.CheckForValidFirstMove( move, stateOfNewPosition, board ))
                return true;

            if(this.CheckForValidCapture( move, stateOfNewPosition ))
                return true;

            if(this.CheckForValidEnPassantMove( move, stateOfNewPosition, board ))
                return true;

            return false;

        }


        public override bool CheckValidMove( Board board, Position newPosition )
        {
            string stateOfNewPosition = board.CheckForPiece( newPosition ); //white,black,none

            Move move = new Move( this.Position, newPosition );

            if(this.ColorOfPieceAtNewPositionIsMyColor( board, newPosition ))
            {
                return false;
            }

            if(CheckIfAnyPawnMoveTypeIsValid( move, stateOfNewPosition, board ))
            {
                return true;
            }

            return false;

        }

    }
}
