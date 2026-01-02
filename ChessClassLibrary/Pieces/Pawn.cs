using System;
using System.Collections.Generic;
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

        private bool CheckIfMoveIsForward( int verticalMove )
        {
            switch(this.Color)
            {
                case "white":
                    if(verticalMove < 0)
                        return true;
                    break;

                case "black":
                    if(verticalMove > 0)
                        return true;
                    break;
            }

            return false;
        }

        private bool CheckNormalMove( int verticalMove, int horizontalMove, string stateOfNewPosition )
        {
            if(Math.Abs( verticalMove ) == 1 && horizontalMove == 0 && stateOfNewPosition == "none")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckFirstMove( int verticalMove, int horizontalMove, string stateOfNewPosition, Board board )
        {
            if(Math.Abs( verticalMove ) == 2 && horizontalMove == 0 && stateOfNewPosition == "none" && this.HasMoved == false)
            {
                Position intermediaryPosition = new Position( this.Position.Row + MoveCloserToZero( verticalMove ), this.Position.Column );
                if( board.CheckForPiece(intermediaryPosition) == "none")
                {
                    return true;
                }
            }

            return false;

        }

        private bool CaptureMove( int verticalMove, int horizontalMove, string stateOfNewPosition )
        {
            if(Math.Abs( verticalMove ) == 1 && Math.Abs( horizontalMove ) == 1 && stateOfNewPosition == this.OpponentColor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool EnPassantMove( int verticalMove, int horizontalMove, string stateOfNewPosition, Position newPosition, Board board )
        {
            if(Math.Abs( verticalMove ) == 1 && Math.Abs( horizontalMove ) == 1 && stateOfNewPosition == "none")
            {

                int enPassantRow = -1;
                int enPassantStartingRow = -1;
                int enPassantCol = newPosition.Column;
                int enPassantStartingCol = newPosition.Column;

                switch(this.Color)
                {
                    case "white":
                        enPassantRow = newPosition.Row + 1;
                        enPassantStartingRow = enPassantRow - 2;
                        break;

                    case "black":
                        enPassantRow = newPosition.Row - 1;
                        enPassantStartingRow = enPassantRow + 2;
                        break;
                }

                Position enPassantPosition = new Position( enPassantRow, enPassantCol );
                Move lastMove = board.ReturnLastMove( board );

                if(board.CheckForPiece( enPassantPosition ) == this.OpponentColor && board.NameOfPiece( enPassantPosition ) == "pawn")
                {

                    if(lastMove.Piece.Name == "pawn" && lastMove.Piece.Color == this.OpponentColor && lastMove.EndingPosition.Row == enPassantRow && lastMove.EndingPosition.Column == enPassantCol)
                    {
                        if(lastMove.StartingPosition.Row == enPassantStartingRow && lastMove.StartingPosition.Column == enPassantStartingCol)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;

        }

        private bool CheckIfUniversalPawnRulesAreValid( int verticalMove, int horizontalMove, string stateOfNewPosition )
        {
            if(stateOfNewPosition == this.Color)
            {
                return false;
            }

            else if(CheckIfMoveIsForward( verticalMove ) == false)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

        private bool CheckIfAnyPawnMoveTypeIsValid( int verticalMove, int horizontalMove, string stateOfNewPosition, Position newPosition, Board board )
        {

            if(this.CheckNormalMove( verticalMove, horizontalMove, stateOfNewPosition ))
            {
                return true;
            }

            else if(this.CheckFirstMove( verticalMove, horizontalMove, stateOfNewPosition, board ))
            {
                return true;
            }

            else if(this.CaptureMove( verticalMove, horizontalMove, stateOfNewPosition ))
            {
                return true;
            }

            else if(this.EnPassantMove( verticalMove, horizontalMove, stateOfNewPosition, newPosition, board ))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public override bool CheckValidMove( Board board, Position newPosition )
        {
            string stateOfNewPosition = board.CheckForPiece( newPosition ); //white,black,none
            int verticalMove = newPosition.Row - Position.Row;
            int horizontalMove = newPosition.Column - Position.Column;

            if(CheckIfUniversalPawnRulesAreValid( verticalMove, horizontalMove, stateOfNewPosition ) == false)
            {
                return false;
            }

            else if(CheckIfAnyPawnMoveTypeIsValid( verticalMove, horizontalMove, stateOfNewPosition, newPosition, board ))
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
