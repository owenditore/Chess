using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Pipelines;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessClassLibrary
{
    public class Piece
    {
        public Piece( string name, string color, int row, int col )
        {
            Position position = new Position( row, col );
            this.Position = position;
            this.Color = color;
            this.Name = name;
            if(color == "white")
            {
                this.OpponentColor = "black";
            }
            else if(color == "black")
            {
                this.OpponentColor = "white";
            }
        }


        protected bool CheckIfMoveDoesNotHitIntermediatePiecesVertically( int move, Board board, Position intermediaryPosition )
        {
            if(Math.Abs( move ) == 1)
            {
                return true;
            }

            do
            {
                move = MoveCloserToZero( move );
                intermediaryPosition.Row = move + this.Position.Row;
                intermediaryPosition.Column = this.Position.Column;
                string stateOfIntermediaryPosition = board.CheckForPiece( intermediaryPosition );
                if(stateOfIntermediaryPosition != "none")
                {
                    return false;
                }

            } while(Math.Abs( move ) != 1);

            return true;
        }

        protected bool CheckIfMoveDoesNotHitIntermediatePiecesDiagonally( int verticalMove, int horizontalMove, Board board, Position intermediaryPosition )
        {
            if(Math.Abs( verticalMove ) == 1)
            {

                return true;
            }
            do
            {
                verticalMove = MoveCloserToZero( verticalMove );
                horizontalMove = MoveCloserToZero( horizontalMove );
                intermediaryPosition.Row = verticalMove + Position.Row;
                intermediaryPosition.Column = horizontalMove + Position.Column;
                string stateOfIntermediaryPosition = board.CheckForPiece( intermediaryPosition );
                if(stateOfIntermediaryPosition != "none")
                {
                    return false;
                }

            } while(Math.Abs( verticalMove ) != 1);


            return true;
        }

        protected bool CheckIfMoveDoesNotHitIntermediatePiecesHorizontally( int move, Board board, Position intermediaryPosition )
        {
            if(Math.Abs( move ) == 1)
            {
                return true;
            }

            do
            {
                move = MoveCloserToZero( move );
                intermediaryPosition.Row = this.Position.Row;
                intermediaryPosition.Column = move + this.Position.Column;
                string stateOfIntermediaryPosition = board.CheckForPiece( intermediaryPosition );
                if(stateOfIntermediaryPosition != "none")
                {
                    return false;
                }

            } while(Math.Abs( move ) != 1);

            return true;
        }

        public bool AllowedToMove( Board board, Position newPosition )
        {
            if(CheckValidMove( board, newPosition ) == true && CheckIfMovePutsSelfInCheck( board, newPosition ) == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AllowedToMoveAnywhere( Board board )
        {
            foreach(var square in board.Squares)
            {
                if(AllowedToMove( board, square.Position ))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckIfKingIsCastlingThroughCheck( Board board, Position newPosition, Position oldPosition )
        {
            Position intermediateKingPosition = new Position( this.Position.Row, -1 );

            if(this.Position.Column > newPosition.Column)
                intermediateKingPosition.Column = this.Position.Column - 1;
            else
                intermediateKingPosition.Column = this.Position.Column + 1;

            //Check If currently in Check
            if(board.IsAttackingTheKingValid())
            {
                return true;
            }

            //Check If intermediary Position is in Check
            this.Position.Row = intermediateKingPosition.Row;
            this.Position.Column = intermediateKingPosition.Column;
            if(board.IsAttackingTheKingValid())
            {
                return true;
            }

            return false;
        }

        public bool CheckIfMovePutsSelfInCheck( Board board, Position newPosition )
        {

            Position oldPosition = new Position( this.Position.Row, this.Position.Column );

            board.CoverToBeCapturedPiece( newPosition );

            //If this piece is a king and is trying to Castle
            if(this.Name == "king" && Math.Abs( this.Position.Column - newPosition.Column ) == 2)
            {
                if(CheckIfKingIsCastlingThroughCheck( board, newPosition, oldPosition ))
                {
                    ResetPiecePosition( board, oldPosition );
                    return true;
                }
            }

            this.Position.Row = newPosition.Row;
            this.Position.Column = newPosition.Column;

            if(board.IsAttackingTheKingValid())
            {
                ResetPiecePosition( board, oldPosition );
                return true;
            }
            else
            {
                ResetPiecePosition( board, oldPosition );
                return false;
            }

        }

        public virtual bool CheckValidMove( Board board, Position newPosition )
        {
            return false;
        }

        public bool IsEqual( Piece otherPiece )
        {
            if(this.PositionsAreEqual( otherPiece ) == true && otherPiece.Name == this.Name && otherPiece.Color == this.Color)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsKingValidMove( Board board )
        {
            if(CheckValidMove( board, board.FindKingPosition() ))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Move( Board board, Position newPosition )
        {

            int numberOfMoves = board.Moves.Count;
            int nextMove = numberOfMoves + 1;

            //En Passant Capture

            if(this.Name == "pawn" && newPosition.Column != this.Position.Column && board.CheckForPiece( newPosition ) == "none")
            {
                Position enPassantCapturePosition = FindEnPassantCapturePosition( newPosition );
                Piece capturedPiece = board.WhatPieceIsHere( enPassantCapturePosition );
                board.Moves.Add( new ChessClassLibrary.Move( nextMove, this, capturedPiece, Position.Row, Position.Column, newPosition.Row, newPosition.Column ) );
                board.Capture( enPassantCapturePosition );
            }

            //Normal Capture
            else if(board.CheckForPiece( newPosition ) != "none")
            {
                Piece capturedPiece = board.WhatPieceIsHere( newPosition );
                board.Moves.Add( new ChessClassLibrary.Move( nextMove, this, capturedPiece, Position.Row, Position.Column, newPosition.Row, newPosition.Column ) );
                board.Capture( newPosition );
            }

            //Rook Castling Move
            else if(this.Name == "king" && Math.Abs( newPosition.Column - this.Position.Column ) == 2)
            {
                board.Moves.Add( new ChessClassLibrary.Move( nextMove, this, Position.Row, Position.Column, newPosition.Row, newPosition.Column, true ) );
                this.CastleRookMove( board, newPosition );
            }

            //Normal Move
            else
            {
                board.Moves.Add( new ChessClassLibrary.Move( nextMove, this, Position.Row, Position.Column, newPosition.Row, newPosition.Column ) );
            }

            this.MoveSquare( board, newPosition );
            Position = newPosition;
            HasMoved = true;

            board.CheckForPromotion();
        }

        private void MoveSquare(Board board, Position newPosition, Piece piece)
        {
            Square? oldSquare = board.Squares.FirstOrDefault( s => s.Position.IsEqual( piece.Position ) );
            oldSquare.Piece = null;

            Square? newSquare = board.Squares.FirstOrDefault( s => s.Position.IsEqual( newPosition ) );
            newSquare.Piece = this;
        }

        private void MoveSquare( Board board, Position newPosition )
        {
            Square? oldSquare = board.Squares.FirstOrDefault( s => s.Position.IsEqual( this.Position ) );
            oldSquare.Piece = null;

            Square? newSquare = board.Squares.FirstOrDefault( s => s.Position.IsEqual( newPosition ) );
            newSquare.Piece = this;
        }

        public bool PositionsAreEqual( Piece otherPiece )
        {
            if(this.Position.IsEqual( otherPiece.Position ))
            {
                return true;
            }

            return false;
        }

        protected int MoveCloserToZero( int number )
        {
            if(number > 0)
            {
                return number - 1;
            }
            else if(number < 0)
            {
                return number + 1;
            }
            else
                return number;
        }

        private void CastleRookMove( Board board, Position newPosition )
        {
            int targetRookColumn = -1;
            int finalRookColumn = -1;
            if(Math.Abs( newPosition.Column - Position.Column ) > 0)
            {
                targetRookColumn = 7;
                finalRookColumn = 5;
            }
            else
            {
                targetRookColumn = 0;
                finalRookColumn = 3;
            }

            Piece? rook = board.Pieces.FirstOrDefault( p =>
                p.Name == "rook" &&
                p.Color == this.Color &&
                p.Position.Row == this.Position.Row &&
                p.Position.Column == targetRookColumn
            );

            Position newRookPosition = new Position( rook.Position.Row, finalRookColumn );
            this.MoveSquare( board, newRookPosition, rook );
            rook.Position.Column = finalRookColumn;
            rook.HasMoved = true;
            
        }

        private Position FindEnPassantCapturePosition( Position newPosition )
        {
            Position capturePosition = new Position( newPosition.Row, newPosition.Column );

            if(this.Color == "white")
            {
                capturePosition.Row++;
            }
            else
            {
                capturePosition.Row--;
            }

            return capturePosition;
        }

        private void ResetPiecePosition( Board board, Position oldPosition )
        {
            Position.Row = oldPosition.Row;
            Position.Column = oldPosition.Column;
            board.UncoverPieces();
        }
        public string Color { get; protected set; }
        public bool Covered { get; set; } = false;
        public bool HasMoved { get; set; } = false;
        public string Name { get; protected set; }
        public string OpponentColor { get; protected set; }
        public Position Position { get; set; }
    }
}
