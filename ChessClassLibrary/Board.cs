using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace ChessClassLibrary
{
    public class Board
    {
        public void AddAPiece( Piece piece )
        {
            foreach(Square square in this.Squares)
            {
                if(square.Position.IsEqual( piece.Position ))
                {
                    square.Piece = piece;
                }
            }
        }

        public void Capture( Position givenPosition )
        {
            Square? square = this.Squares.FirstOrDefault( s => s.Position.IsEqual( givenPosition ) );
            square.Piece = null;

        }

        public void Capture( Piece givenPiece )
        {
            Square? square = this.Squares.FirstOrDefault( s => s.Piece.IsEqual( givenPiece ) );
            square.Piece = null;
        }

        public void CheckForPawnToPromote()
        {
            var squaresWithPieces = this.Squares.Where( s => s.Piece != null );
            Square? squareWithPawnToPromote = squaresWithPieces.FirstOrDefault( s => s.Piece.NeedToPromote() == true );

            if(squareWithPawnToPromote != null)
            {
                this.APawnNeedsToPromote = true;
            }

        }

        public string CheckForPiece( Position position )
        {
            Square? square = this.Squares.FirstOrDefault( s => s.Position.IsEqual( position ));

            if(square.Piece == null)
                return "none";

            if(square.Piece.Covered == true)
                return "none";
            
            return square.Piece.Color;

        }

        public Piece TemporarilyRemoveToBeCapturedPiece( Position position )
        {

            Square? square = this.Squares.FirstOrDefault( s => s.Position.IsEqual( position ) );

            return square.Piece;

        }

        public void ReplaceTemporarilyRemovedToBeCapturedPiece( Position position, Piece piece )
        {
            Square? square = this.Squares.FirstOrDefault( s => s.Position.IsEqual( position ) );

            square.Piece = piece;
        }

        public Position FindKingPosition()
        {
            Piece? king = _parentGame.CurrentPlayer.Pieces.FirstOrDefault( p => p.Name == "king" );
            return king.Position;
        }

        public bool IsPathToNewPositionClear( Move move )
        {
            string direction = move.IsMoveVerticalHorizontalOrDiagonal();

            switch(direction)
            {
                case "vertical":
                    return IsVerticalPathToNewPositionClear( move );


                case "horizontal":
                    return IsHorizontalPathToNewPositionClear( move );


                case "diagonal":
                    return IsDiagonalPathToNewPositionClear( move );
            }

            return false;
        }

        public bool IsTheKingInCheck()
        {
            foreach(Piece piece in _parentGame.NotCurrentPlayer.Pieces)
            {
                if(piece.Covered == false && piece.CheckValidMove( this, this.FindKingPosition() ))
                {
                    return true;
                }
            }
            return false;
        }

        public void UncoverPieces()
        {
            foreach(Square square in this.Squares)
            {
                if(square.Piece == null)
                {
                    continue;
                }
                if(square.Piece.Covered == true)
                {
                    square.Piece.Covered = false;
                }
            }
        }

        public Piece WhatPieceIsHere( Position newPosition )
        {
            Square? square = this.Squares.FirstOrDefault( s =>
                s.Position.IsEqual( newPosition )
            );

            return square.Piece;
        }

        private bool IsDiagonalPathToNewPositionClear( Move move )
        {
            if(Math.Abs( move.Vertical ) == 1) return true;

            Position intermediaryPosition = new Position( -1, -1 );
            int verticalMoveLength = move.Vertical;
            int horizontalMoveLength = move.Horizontal;
            do
            {
                verticalMoveLength = move.DistanceCloserToZero( verticalMoveLength );
                horizontalMoveLength = move.DistanceCloserToZero( horizontalMoveLength );
                intermediaryPosition.Row = verticalMoveLength + move.StartingPosition.Row;
                intermediaryPosition.Column = horizontalMoveLength + move.StartingPosition.Column;
                string stateOfIntermediaryPosition = this.CheckForPiece( intermediaryPosition );
                if(stateOfIntermediaryPosition != "none")
                {
                    return false;
                }

            } while(Math.Abs( verticalMoveLength ) != 1);

            return true;
        }

        private bool IsHorizontalPathToNewPositionClear( Move move )
        {
            if(Math.Abs( move.Horizontal ) == 1) return true;

            Position intermediaryPosition = new Position( -1, -1 );
            int moveLength = move.Horizontal;
            do
            {
                moveLength = move.DistanceCloserToZero( moveLength );
                intermediaryPosition.Row = move.StartingPosition.Row;
                intermediaryPosition.Column = moveLength + move.StartingPosition.Column;
                string stateOfIntermediaryPosition = this.CheckForPiece( intermediaryPosition );
                if(stateOfIntermediaryPosition != "none")
                {
                    return false;
                }

            } while(Math.Abs( moveLength ) != 1);

            return true;
        }

        private bool IsVerticalPathToNewPositionClear( Move move )
        {
            if(Math.Abs( move.Vertical ) == 1) return true;

            Position intermediaryPosition = new Position( -1, -1 );
            int moveLength = move.Vertical;
            do
            {
                moveLength = move.DistanceCloserToZero( moveLength );
                intermediaryPosition.Row = moveLength + move.StartingPosition.Row;
                intermediaryPosition.Column = move.StartingPosition.Column;
                string stateOfIntermediaryPosition = this.CheckForPiece( intermediaryPosition );
                if(stateOfIntermediaryPosition != "none")
                {
                    return false;
                }

            } while(Math.Abs( moveLength ) != 1);

            return true;
        }

        private void CreateSquares()
        {
            for(int row = 0; row < 8; row++)
            {
                for(int column = 0; column < 8; column++)
                {
                    this.Squares.Add( new Square( row, column ) );
                }
            }
        }

        public void AddPiecesToBoard()
        {
            foreach(Piece piece in this._parentGame.WhitePlayer.Pieces)
            {
                AddAPiece(piece);
            }
            foreach(Piece piece in this._parentGame.BlackPlayer.Pieces)
            {
                AddAPiece( piece );
            }

        }


        public bool APawnNeedsToPromote { get; set; } = false;

        public List<Square> Squares { get; set; } = new List<Square>();

        public readonly Game _parentGame;

        public Board(Game parentGame)
        {
            _parentGame = parentGame;
            CreateSquares();
        }
    }
}
