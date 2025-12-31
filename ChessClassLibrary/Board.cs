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

        public void CoverToBeCapturedPiece( Position position )
        {
            foreach(Piece piece in this.Pieces)
            {
                if(piece.Position.IsEqual( position ))
                {
                    piece.Covered = true;
                }
            }
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

        public bool IsAttackingTheKingValid()
        {
            foreach(Piece piece in this.Pieces)
            {
                if(piece.Covered == false && piece.CheckValidMove(this, this.FindKingPosition()))
                {
                    return true;
                }
            }
            return false;
        }

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

        public void AddFEN( Board board )
        {
            FENs.Add( new FenNotation( board ) );
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

        public bool AnyPieceAllowedToMove()
        {
            foreach(Piece piece in this.Pieces)
            {
                if(piece.Color == this.Turn && piece.AllowedToMoveAnywhere( this ) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public string CheckForCheckOrStaleMate()
        {
            if(AnyPieceAllowedToMove())
            {
                return "";
            }

            foreach(Piece piece in this.Pieces)
            {
                if(piece.Color != Turn && piece.IsKingValidMove( this ))
                {
                    return "checkmate";
                }
            }

            return "stalemate";
        }

        private Dictionary<string, int> CountTypesOfPieces()
        {
            int numberOfQueens = 0;
            int numberOfRooks = 0;
            int numberOfPawns = 0;
            int numberOfWhiteBishops = 0;
            int numberOfBlackBishops = 0;
            int numberOfWhiteKnights = 0;
            int numberOfBlackKnights = 0;

            foreach(Piece piece in Pieces)
            {
                switch(piece.Name)
                {
                    case "queen":
                        numberOfQueens++;
                        break;

                    case "rook":
                        numberOfRooks++;
                        break;

                    case "pawn":
                        numberOfPawns++;
                        break;

                    case "bishop":
                        if(piece.Color == "white") numberOfWhiteBishops++;
                        else if(piece.Color == "black") numberOfBlackBishops++;
                        break;

                    case "knight":
                        if(piece.Color == "white") numberOfWhiteKnights++;
                        else if(piece.Color == "black") numberOfBlackKnights++;
                        break;

                    default:
                        break;
                }
            }

            var numberOfPieces = new Dictionary<string, int>
            {
                { "queen", numberOfQueens },
                { "rook", numberOfRooks },
                { "pawn", numberOfPawns },
                { "whiteBishop", numberOfWhiteBishops },
                { "blackBishop", numberOfBlackBishops },
                { "whiteKnight", numberOfWhiteKnights },
                { "blackKnight", numberOfBlackKnights }
            };

            return numberOfPieces;
        }

        public string CheckForInsufficientMaterial()
        {
            Dictionary<string, int> numberOfPieces = this.CountTypesOfPieces();

            if(numberOfPieces["queen"] > 0) return "";
            if(numberOfPieces["rook"] > 0) return "";
            if(numberOfPieces["pawn"] > 0) return "";

            if(numberOfPieces["whiteBishop"] > 1) return "";
            if(numberOfPieces["blackBishop"] > 1) return "";

            if(numberOfPieces["whiteKnight"] > 1) return "";
            if(numberOfPieces["blackKnight"] > 1) return "";

            if(numberOfPieces["whiteBishop"] > 0 && numberOfPieces["whiteKnight"] > 0) return "";
            if(numberOfPieces["blackBishop"] > 0 && numberOfPieces["blackKnight"] > 0) return "";

            return "draw";
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

        public string CheckForPiece( Position position )
        {
            foreach(Piece piece in Pieces)
            {
                if(piece.Position.IsEqual( position ) == true && piece.Covered == false)
                {
                    return piece.Color;
                }

            }
            return "none";
        }

        public Piece CheckForPieceToPromoteTo( Position clickedPosition )
        {
            Piece? pieceToPromoteTo = this.PromotionList.FirstOrDefault( p =>
                p.Position.IsEqual( clickedPosition ) &&
                p.Color == this.Turn
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

        public void CheckForPromotion()
        {
            Piece? whitePawnToPromote = this.Pieces.FirstOrDefault( p =>
                p.Color == this.Turn &&
                p.Color == "white" &&
                p.Name == "pawn" &&
                p.Position.Row == 0
            );

            Piece? blackPawnToPromote = this.Pieces.FirstOrDefault( p =>
                p.Color == this.Turn &&
                p.Color == "black" &&
                p.Name == "pawn" &&
                p.Position.Row == 0
            );

            if(whitePawnToPromote != null || blackPawnToPromote != null)
            {
                NeedToPromote = true;
            }
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
            foreach(Piece piece in Pieces)
            {
                if(piece.Name == "king" && piece.Color == Turn)
                {
                    return piece.Position;
                }
            }
            return null;
        }

        public string NameOfPiece( Position position )
        {
            foreach(Piece piece in Pieces)
            {
                int posRow = position.Row;
                int posCol = position.Column;
                int pieceRow = piece.Position.Row;
                int pieceCol = piece.Position.Column;

                if(pieceRow == posRow && pieceCol == posCol)
                {
                    if(piece.Covered == false)
                        return piece.Name;
                }

            }
            return null;
        }

        public void NextTurn()
        {
            if(Turn == "white")
                Turn = "black";
            else
                Turn = "white";
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

        public Move ReturnLastMove( Board board )
        {
            int numberOfMoves = board.Moves.Count();
            foreach(Move move in board.Moves)
            {
                if(move.Number == numberOfMoves)
                {
                    return move;
                }
            }
            return null;
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

        public Piece WhatPieceIsHere( Position newPosition )
        {
            Piece? piece = this.Pieces.FirstOrDefault( p =>
                p.Position.IsEqual( newPosition )
            );

            return piece;
        }

        public List<FenNotation> FENs { get; set; } = new List<FenNotation>();
        public List<Move> Moves { get; set; } = new List<Move>();
        public bool NeedToPromote { get; set; } = false;
        public List<Piece> Pieces { get; set; } = new List<Piece>();

        public List<Piece> PromotionList { get; set; } = new List<Piece>();
        public List<Square> Squares { get; set; } = new List<Square>();
        public string Turn { get; set; } = "white";
    }
}
