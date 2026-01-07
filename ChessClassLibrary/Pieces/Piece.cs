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
        public bool AllowedToMove( Board board, Position newPosition )
        {
            if(this.CheckValidMove( board, newPosition ) == false)
                return false;

            if(this.CheckIfMovePutsSelfInCheck( board, newPosition ))
                return false;

            return true;

        }

        public bool AllowedToMoveAnywhere( Board board )
        {
            foreach(var square in board.Squares)
            {
                if(this.AllowedToMove( board, square.Position ))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckIfMovePutsSelfInCheck( Board board, Position newPosition )
        {

            Position oldPosition = new Position( this.Position.Row, this.Position.Column );

            //Covering a Piece that is about to be capture let's the object check if the king is in check without considering the covered piece. If it is in check you then uncover
            Piece? toBeCapturedPiece = board.TemporarilyRemoveToBeCapturedPiece( newPosition );

            if(this.PieceIsKingTryingToCastle( newPosition ))
            {
                if(this.KingIsCastlingThroughCheck( board, newPosition, oldPosition ))
                {
                    return true;
                }
            }

            this.MoveSquare( board, newPosition );
            this.Position.Row = newPosition.Row;
            this.Position.Column = newPosition.Column;

            if(board.IsTheKingInCheck())
            {
                ResetPiecePosition( board, oldPosition, toBeCapturedPiece );
                return true;
            }
            else
            {
                ResetPiecePosition( board, oldPosition, toBeCapturedPiece );
                return false;
            }

        }

        public virtual bool CheckValidMove( Board board, Position newPosition )
        {
            return false;
        }

        public bool IsARookThatCanCastle()
        {
            if(this.Name != "rook") return false;

            if(this.HasMoved) return false;

            return true;
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

        public bool KingIsCastlingThroughCheck( Board board, Position newPosition, Position oldPosition )
        {

            if(board.IsTheKingInCheck())
            {
                return true;
            }

            Position intermediateKingPosition = new Position( this.Position.Row, -1 );

            if(this.Position.Column > newPosition.Column)
                intermediateKingPosition.Column = this.Position.Column - 1;
            else
                intermediateKingPosition.Column = this.Position.Column + 1;

            this.MoveSquare( board, newPosition );

            if(board.IsTheKingInCheck())
            {
                this.ResetPiecePosition( board, oldPosition );
                return true;
            }

            this.ResetPiecePosition( board, oldPosition );
            return false;
        }

        public void MovePiece( Game game, Position newPosition )
        {

            int numberOfTurns = game.Turns.Count;
            int nextTurn = numberOfTurns + 1;

            if(this.PieceIsAPawnMovingEnPassant( game.Board, newPosition ))
            {
                Position enPassantCapturePosition = FindEnPassantCapturePosition( newPosition );
                Piece capturedPiece = game.Board.WhatPieceIsHere( enPassantCapturePosition );
                game.Turns.Add( new ChessClassLibrary.Turn( nextTurn, this, capturedPiece, Position.Row, Position.Column, newPosition.Row, newPosition.Column ) );
                game.Capture( enPassantCapturePosition );
            }

            else if(this.PieceIsCapturingNormally( game.Board, newPosition ))
            {
                Piece capturedPiece = game.Board.WhatPieceIsHere( newPosition );
                game.Turns.Add( new ChessClassLibrary.Turn( nextTurn, this, capturedPiece, Position.Row, Position.Column, newPosition.Row, newPosition.Column ) );
                game.Capture( newPosition );
            }

            else if(this.PieceIsKingTryingToCastle( newPosition ))
            {
                game.Turns.Add( new ChessClassLibrary.Turn( nextTurn, this, Position.Row, Position.Column, newPosition.Row, newPosition.Column, true ) );
                this.CastleRookMove( game.Board, newPosition );
            }

            else //Piece Is Moving Normally Without Capturing
            {
                game.Turns.Add( new ChessClassLibrary.Turn( nextTurn, this, Position.Row, Position.Column, newPosition.Row, newPosition.Column ) );
            }

            this.MoveSquare( game.Board, newPosition );
            Position = newPosition;
            HasMoved = true;

            game.Board.CheckForPawnToPromote();
        }

        public virtual bool NeedToPromote()
        {
            return false;
        }

        public bool PositionsAreEqual( Piece otherPiece )
        {
            if(this.Position.IsEqual( otherPiece.Position ))
            {
                return true;
            }

            return false;
        }

        protected bool ColorOfPieceAtNewPositionIsMyColor( Board board, Position newPosition )
        {
            if(this.Color == board.CheckForPiece( newPosition ))
                return true;

            return false;
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

            Position targetRookPosition = new Position( this.Position.Row, targetRookColumn );
            Position finalRookPosition = new Position( this.Position.Row, finalRookColumn );

            Square? square = board.Squares.FirstOrDefault( s => s.Position.IsEqual( targetRookPosition ) );
            if(square.Piece.IsARookThatCanCastle() == false)
            {
                return;
            }

            square.Piece.HasMoved = true;
            this.MoveSquare( board, finalRookPosition, square.Piece );

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

        private void MoveSquare( Board board, Position newPosition, Piece piece )
        {
            Square? oldSquare = board.Squares.FirstOrDefault( s => s.Position.IsEqual( piece.Position ) );
            oldSquare.Piece = null;

            Square? newSquare = board.Squares.FirstOrDefault( s => s.Position.IsEqual( newPosition ) );
            newSquare.Piece = this;

            newSquare.Piece.Position = newPosition;
        }
        private void MoveSquare( Board board, Position newPosition )
        {
            Square? oldSquare = board.Squares.FirstOrDefault( s => s.Position.IsEqual( this.Position ) );
            oldSquare.Piece = null;

            Square? newSquare = board.Squares.FirstOrDefault( s => s.Position.IsEqual( newPosition ) );
            newSquare.Piece = this;

            newSquare.Piece.Position = newPosition;
        }

        private bool PieceIsAPawnMovingEnPassant( Board board, Position newPosition )
        {
            if(this.Name != "pawn")
                return false;

            if(this.Position.Column == newPosition.Column)
                return false;

            if(board.CheckForPiece( newPosition ) != "none")
                return false;

            return true;
        }

        private bool PieceIsCapturingNormally( Board board, Position newPosition )
        {
            if(board.CheckForPiece( newPosition ) != "none")
                return true;

            return false;
        }

        private bool PieceIsKingTryingToCastle( Position newPosition )
        {
            if(this.Name != "king")
                return false;

            if(Math.Abs( this.Position.Column - newPosition.Column ) != 2)
                return false;

            return true;

        }
        private void ResetPiecePosition( Board board, Position oldPosition )
        {
            this.MoveSquare( board, oldPosition );
        }

        private void ResetPiecePosition( Board board, Position oldPosition, Piece tempCapturedPiece )
        {
            this.MoveSquare( board, oldPosition );
            board.ReplaceTemporarilyRemovedToBeCapturedPiece( this.Position, tempCapturedPiece );
        }

        public string Color { get; protected set; }

        public bool Covered { get; set; } = false;

        public bool HasMoved { get; set; } = false;

        public string Name { get; set; }

        public string OpponentColor { get; protected set; }

        public Position Position { get; set; }

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
    }
}
