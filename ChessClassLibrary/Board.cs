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
            this.Pieces.Add( piece );

            foreach(Square square in this.Squares)
            {
                if(square.Position.IsEqual( piece.Position ))
                {
                    square.Piece = piece;
                }
            }
        }

        public void AddFEN()
        {
            FENs.Add( new FenNotation( this ) );
        }

        public string GetCurrentFEN()
        {
            FenNotation currentFEN = FENs.Last();
            return currentFEN.fen;
        }

        public bool AnyPieceAllowedToMove()
        {
            foreach(Piece piece in this.Pieces)
            {
                if(piece.Color == this.CurrentTurnColor && piece.AllowedToMoveAnywhere( this ) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public void Capture( Position givenPosition )
        {

            this.Pieces.RemoveAll( piece => piece.Position.IsEqual( givenPosition ) );

            Square? square = this.Squares.FirstOrDefault( s => s.Position.IsEqual( givenPosition ) );
            square.Piece = null;

        }

        public void Capture( Piece givenPiece )
        {
            this.Pieces.RemoveAll( piece => piece.IsEqual( givenPiece ) );
            Square? square = this.Squares.FirstOrDefault( s => s.Position.IsEqual( givenPiece.Position ) );
            square.Piece = null;
        }

        public string CheckForCheckOrStaleMate()
        {
            if(AnyPieceAllowedToMove())
            {
                return "";
            }

            if(IsTheKingInCheck())
            {
                return "checkmate";
            }

            return "stalemate";
        }

        private Dictionary<string,int> FindNumberOfEachTypeOfPiece()
        {
            var numbersOfPieces = new Dictionary<string, int>
            {
                {"queen", this.Pieces.Count( p => p.Name == "queen" ) },
                {"rook", this.Pieces.Count( p => p.Name == "rook" ) },
                {"pawn", this.Pieces.Count( p => p.Name == "pawn" ) },

                {"whiteBishop", this.Pieces.Count( p => p.Name == "bishop" && p.Color == "white" ) },
                {"blackBishop", this.Pieces.Count( p => p.Name == "bishop" && p.Color == "black" ) },

                {"whiteKnight", this.Pieces.Count( p => p.Name == "knight" && p.Color == "white" ) },
                {"blackKnight", this.Pieces.Count( p => p.Name == "knight" && p.Color == "black" ) },
            };

            return numbersOfPieces;
        }

        public string CheckForInsufficientMaterial()
        {
            var numbersOfPieces = FindNumberOfEachTypeOfPiece();

            if(EitherPlayerHasAnyQueensRooksOrPawns( numbersOfPieces )) return "";

            if(EitherPlayerHasTwoBishopsOrTwoKnights( numbersOfPieces )) return "";

            if(EitherPlayerHasABishopAndAKnight( numbersOfPieces )) return "";

            return "draw";
        }

        private bool EitherPlayerHasAnyQueensRooksOrPawns( Dictionary<string,int> numbersOfPieces )
        {

            int totalQRPs = numbersOfPieces["pawn"] + numbersOfPieces["rook"] + numbersOfPieces["queen"];

            if(totalQRPs != 0) 
                return true;

            return false;
        }
        private bool EitherPlayerHasTwoBishopsOrTwoKnights( Dictionary<string, int> numbersOfPieces )
        {

            if(numbersOfPieces["whiteKnight"] > 1) return true;
            if(numbersOfPieces["blackKnight"] > 1) return true;
            if(numbersOfPieces["whiteBishop"] > 1) return true;
            if(numbersOfPieces["blackBishop"] > 1) return true;

            return false;

        }
        private bool EitherPlayerHasABishopAndAKnight( Dictionary<string, int> numbersOfPieces )
        {

            if(numbersOfPieces["whiteKnight"] + numbersOfPieces["whiteBishop"] > 1) return true;
            if(numbersOfPieces["blackKnight"] + numbersOfPieces["blackBishop"] > 1) return true;

            return false;
        }

        public string CheckForMateOrDraw()
        {
            string checkForMate = this.CheckForCheckOrStaleMate();
            if(checkForMate == "checkmate") return "checkmate";
            if(checkForMate == "stalemate") return "draw";

            string checkForInsufficientMaterial = this.CheckForInsufficientMaterial();
            if(checkForInsufficientMaterial == "draw") return "draw";

            //string checkForThreeRepitition = CheckForThreeRepitition(board);
            //if (checkForThreeRepitition == "threerepitition") return checkForThreeRepitition;

            //string checkForFiftyMoves = CheckForFiftyMoves(board);
            //if (checkForFiftyMoves == "fiftymoves") return checkForFiftyMoves;

            return "";
        }

        public void CheckForPawnToPromote()
        {

            Piece? pawnToPromote = this.Pieces.FirstOrDefault( p => p.NeedToPromote() == true );

            if(pawnToPromote != null)
            {
                this.APawnNeedsToPromote = true;
            }

        }

        public string CheckForPiece( Position position )
        {
            Piece? piece = this.Pieces.FirstOrDefault( p =>
                p.Covered == false &&
                p.Position.IsEqual( position )
            );

            if(piece != null)
                return piece.Color;

            return "none";

        }

        public Piece CheckForPieceToPromoteTo( Position clickedPosition )
        {
            Piece? pieceToPromoteTo = this.PromotionList.FirstOrDefault( p =>
                p.Position.IsEqual( clickedPosition ) &&
                p.Color == this.CurrentTurnColor
            );

            return pieceToPromoteTo;
        }

        public Piece CheckForPromotablePiece()
        {
            Piece? promoteFromPiece = this.Pieces.FirstOrDefault( p =>
                ( p.Name == "pawn" &&
                p.Position.Row == 7 ) ||
                ( p.Name == "pawn" &&
                p.Position.Row == 0 )
            );
            return promoteFromPiece;
        }

        public void CoverToBeCapturedPiece( Position position )
        {
            Piece? piece = this.Pieces.FirstOrDefault(p => p.Position.IsEqual( position ));

            if(piece != null)
                piece.Covered = true;

        }

        public void CreatePromotionList()
        {
            Queen whitePromoteQueen = new Queen( "queen", "white", 2, 7 );
            Rook whitePromoteRook = new Rook( "rook", "white", 3, 7 );
            Bishop whitePromoteBishop = new Bishop( "bishop", "white", 4, 7 );
            Knight whitePromoteKnight = new Knight( "knight", "white", 5, 7 );

            Queen blackPromoteQueen = new Queen( "queen", "black", 2, 0 );
            Rook blackPromoteRook = new Rook( "rook", "black", 3, 0 );
            Bishop blackPromoteBishop = new Bishop( "bishop", "black", 4, 0 );
            Knight blackPromoteKnight = new Knight( "knight", "black", 5, 0 );

            PromotionList.Add( whitePromoteQueen );
            PromotionList.Add( whitePromoteRook );
            PromotionList.Add( whitePromoteBishop );
            PromotionList.Add( whitePromoteKnight );

            PromotionList.Add( blackPromoteQueen );
            PromotionList.Add( blackPromoteRook );
            PromotionList.Add( blackPromoteBishop );
            PromotionList.Add( blackPromoteKnight );

        }

        public Position FindKingPosition()
        {
            Piece? king = this.Pieces.FirstOrDefault(p => 
                p.Name == "king" &&
                p.Color == this.CurrentTurnColor
            );

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
            foreach(Piece piece in this.Pieces)
            {
                if(piece.Covered == false && piece.CheckValidMove( this, this.FindKingPosition() ))
                {
                    return true;
                }
            }
            return false;
        }

        public void NextTurn()
        {
            if(CurrentTurnColor == "white")
                CurrentTurnColor = "black";
            else
            {
                CurrentTurnColor = "white";
                FullMoveNumber++;
            }

            IncrementHalfMoveClock();
        }

        private void IncrementHalfMoveClock()
        {
            Turn? lastTurn = this.Turns.Last();

            if( lastTurn.Piece.Name == "pawn")
            {
                HalfMoveClock = 0;
                return;
            }

            if(lastTurn.CapturedPiece != null)
            {
                HalfMoveClock = 0;
                return;
            }

            HalfMoveClock++;
        }

        public void PromotePiece( Piece promotablePiece, Piece promoteToPiece )
        {
            switch(promoteToPiece.Name)
            {
                case "queen":
                    this.Pieces.Add( new Queen( promoteToPiece.Name, promoteToPiece.Color, promotablePiece.Position.Row, promotablePiece.Position.Column ) );
                    break;
                case "rook":
                    this.Pieces.Add( new Rook( promoteToPiece.Name, promoteToPiece.Color, promotablePiece.Position.Row, promotablePiece.Position.Column ) );
                    break;
                case "bishop":
                    this.Pieces.Add( new Bishop( promoteToPiece.Name, promoteToPiece.Color, promotablePiece.Position.Row, promotablePiece.Position.Column ) );
                    break;
                case "knight":
                    this.Pieces.Add( new Knight( promoteToPiece.Name, promoteToPiece.Color, promotablePiece.Position.Row, promotablePiece.Position.Column ) );
                    break;
                default:
                    break;
            }

            this.Capture( promotablePiece );
        }

        public Turn ReturnLastTurn( Board board )
        {
            int numberOfTurns = board.Turns.Count();

            return board.Turns.FirstOrDefault(t => t.Number == numberOfTurns );

        }

        public void SetupGame()
        {
            Pawn whitePawnA = new Pawn( "pawn", "white", 6, 0 );
            Pawn whitePawnB = new Pawn( "pawn", "white", 6, 1 );
            Pawn whitePawnC = new Pawn( "pawn", "white", 6, 2 );
            Pawn whitePawnD = new Pawn( "pawn", "white", 6, 3 );
            Pawn whitePawnE = new Pawn( "pawn", "white", 6, 4 );
            Pawn whitePawnF = new Pawn( "pawn", "white", 6, 5 );
            Pawn whitePawnG = new Pawn( "pawn", "white", 6, 6 );
            Pawn whitePawnH = new Pawn( "pawn", "white", 6, 7 );

            Rook whiteRookA = new Rook( "rook", "white", 7, 0 );
            Rook whiteRookH = new Rook( "rook", "white", 7, 7 );

            Knight whiteKnightB = new Knight( "knight", "white", 7, 1 );
            Knight whiteKnightG = new Knight( "knight", "white", 7, 6 );

            Bishop whiteBishopC = new Bishop( "bishop", "white", 7, 2 );
            Bishop whiteBishopF = new Bishop( "bishop", "white", 7, 5 );

            Queen whiteQueen = new Queen( "queen", "white", 7, 3 );
            King whiteKing = new King( "king", "white", 7, 4 );

            Pawn blackPawnA = new Pawn( "pawn", "black", 1, 0 );
            Pawn blackPawnB = new Pawn( "pawn", "black", 1, 1 );
            Pawn blackPawnC = new Pawn( "pawn", "black", 1, 2 );
            Pawn blackPawnD = new Pawn( "pawn", "black", 1, 3 );
            Pawn blackPawnE = new Pawn( "pawn", "black", 1, 4 );
            Pawn blackPawnF = new Pawn( "pawn", "black", 1, 5 );
            Pawn blackPawnG = new Pawn( "pawn", "black", 1, 6 );
            Pawn blackPawnH = new Pawn( "pawn", "black", 1, 7 );

            Rook blackRookA = new Rook( "rook", "black", 0, 0 );
            Rook blackRookH = new Rook( "rook", "black", 0, 7 );

            Knight blackKnightB = new Knight( "knight", "black", 0, 1 );
            Knight blackKnightG = new Knight( "knight", "black", 0, 6 );

            Bishop blackBishopC = new Bishop( "bishop", "black", 0, 2 );
            Bishop blackBishopF = new Bishop( "bishop", "black", 0, 5 );

            Queen blackQueen = new Queen( "queen", "black", 0, 3 );
            King blackKing = new King( "king", "black", 0, 4 );

            AddAPiece( whitePawnA );
            AddAPiece( whitePawnB );
            AddAPiece( whitePawnC );
            AddAPiece( whitePawnD );
            AddAPiece( whitePawnE );
            AddAPiece( whitePawnF );
            AddAPiece( whitePawnG );
            AddAPiece( whitePawnH );
            AddAPiece( whiteRookA );
            AddAPiece( whiteRookH );
            AddAPiece( whiteKnightB );
            AddAPiece( whiteKnightG );
            AddAPiece( whiteBishopC );
            AddAPiece( whiteBishopF );
            AddAPiece( whiteQueen );
            AddAPiece( whiteKing );

            AddAPiece( blackPawnA );
            AddAPiece( blackPawnB );
            AddAPiece( blackPawnC );
            AddAPiece( blackPawnD );
            AddAPiece( blackPawnE );
            AddAPiece( blackPawnF );
            AddAPiece( blackPawnG );
            AddAPiece( blackPawnH );
            AddAPiece( blackRookA );
            AddAPiece( blackRookH );
            AddAPiece( blackKnightB );
            AddAPiece( blackKnightG );
            AddAPiece( blackBishopC );
            AddAPiece( blackBishopF );
            AddAPiece( blackQueen );
            AddAPiece( blackKing );
            CreatePromotionList();

        }

        public void UncoverPieces()
        {
            foreach(Piece piece in this.Pieces)
            {
                if(piece.Covered == true)
                {
                    piece.Covered = false;
                }
            }
        }

        public Piece WhatPieceIsHere( Position newPosition )
        {
            Piece? piece = this.Pieces.FirstOrDefault( p =>
                p.Position.IsEqual( newPosition )
            );

            return piece;
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

        public bool APawnNeedsToPromote { get; set; } = false;

        public string CurrentTurnColor { get; set; } = "white";

        public List<FenNotation> FENs { get; set; } = new List<FenNotation>();

        public List<Piece> Pieces { get; set; } = new List<Piece>();

        public List<Piece> PromotionList { get; set; } = new List<Piece>();

        public List<Square> Squares { get; set; } = new List<Square>();

        public List<Turn> Turns { get; set; } = new List<Turn>();

        public int HalfMoveClock { get; set; } = 0;

        public int FullMoveNumber { get; set; } = 1;

        public Board()
        {
            for(int row = 0; row < 8; row++)
            {
                for(int column = 0; column < 8; column++)
                {
                    this.Squares.Add( new Square( row, column ) );
                }
            }
        }
    }
}
