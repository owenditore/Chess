using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

namespace ChessClassLibrary
{
    public class Board
    {
        public List<Piece> Pieces { get; set; } = new List<Piece>();

        public List<Piece> PromotionList { get; set; } = new List<Piece>();

        public List<Move> Moves { get; set; } = new List<Move>();

        public bool Check { get; set; } = false;
        public bool CheckMate { get; set; } = false;

        public string Turn { get; set; } = "white";

        public List<Position> Positions { get; set; } = new List<Position>();

        public bool Promotion { get; set; } = false;

        public Board()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    Positions.Add(new Position(r, c));
                }
            }
        }

        public void CreatePromotionList()
        {
            Queen whitePromoteQueen = new Queen("queen", "white", 2, 7);
            Rook whitePromoteRook = new Rook("rook", "white", 3, 7);
            Bishop whitePromoteBishop = new Bishop("bishop", "white", 4, 7);
            Knight whitePromoteKnight = new Knight("knight", "white", 5, 7);

            Queen blackPromoteQueen = new Queen("queen", "black", 2, 0);
            Rook blackPromoteRook = new Rook("rook", "black", 3, 0);
            Bishop blackPromoteBishop = new Bishop("bishop", "black", 4, 0);
            Knight blackPromoteKnight = new Knight("knight", "black", 5, 0);

            PromotionList.Add(whitePromoteQueen);
            PromotionList.Add(whitePromoteRook);
            PromotionList.Add(whitePromoteBishop);
            PromotionList.Add(whitePromoteKnight);

            PromotionList.Add(blackPromoteQueen);
            PromotionList.Add(blackPromoteRook);
            PromotionList.Add(blackPromoteBishop);
            PromotionList.Add(blackPromoteKnight);

        }


        public void TestCastle()
        {
            Rook whiteRookA = new Rook("rook", "white", 7, 0);
            Rook whiteRookH = new Rook("rook", "white", 7, 7);
            King whiteKing = new King("king", "white", 7, 4);
            Rook blackRook = new Rook("rook", "black", 3, 3);
            King blackKing = new King("king", "black", 0, 4);

            Pieces.Add(whiteRookA);
            Pieces.Add(whiteRookH);
            Pieces.Add(whiteKing);
            Pieces.Add(blackRook);
            Pieces.Add(blackKing);
            CreatePromotionList();
        }

        public void TestSetup()
        {
            Pawn testPawn1 = new Pawn("pawn","white", 3, 3);
            Pawn testPawn2 = new Pawn("pawn", "black", 6, 6);
            Bishop testBishop1 = new Bishop("bishop", "white", 5, 5);
            Rook testRook1 = new Rook("rook", "white", 1, 1);
            Queen testQueen1 = new Queen("queen", "white", 0, 0);
            Knight testKnight1 = new Knight("knight", "black", 0, 7);
            King testKing1 = new King("king", "white", 7, 0);
            King testKing2 = new King("king", "black", 5, 7);
            Pieces.Add(testPawn1);
            Pieces.Add(testPawn2);
            Pieces.Add(testBishop1);
            Pieces.Add(testRook1);
            Pieces.Add(testQueen1);
            Pieces.Add(testKnight1);
            Pieces.Add(testKing1);
            Pieces.Add(testKing2);
            CreatePromotionList();
        }

        public void SetupGame()
        {
            Pawn whitePawnA = new Pawn("pawn", "white", 6, 0);
            Pawn whitePawnB = new Pawn("pawn", "white", 6, 1);
            Pawn whitePawnC = new Pawn("pawn", "white", 6, 2);
            Pawn whitePawnD = new Pawn("pawn", "white", 6, 3);
            Pawn whitePawnE = new Pawn("pawn", "white", 6, 4);
            Pawn whitePawnF = new Pawn("pawn", "white", 6, 5);
            Pawn whitePawnG = new Pawn("pawn", "white", 6, 6);
            Pawn whitePawnH = new Pawn("pawn", "white", 6, 7);

            Rook whiteRookA = new Rook("rook", "white", 7, 0);
            Rook whiteRookH = new Rook("rook", "white", 7, 7);

            Knight whiteKnightB = new Knight("knight", "white", 7, 1);
            Knight whiteKnightG = new Knight("knight", "white", 7, 6);

            Bishop whiteBishopC = new Bishop("bishop", "white", 7, 2);
            Bishop whiteBishopF = new Bishop("bishop", "white", 7, 5);

            Queen whiteQueen = new Queen("queen", "white", 7, 3);
            King whiteKing = new King("king", "white", 7, 4);

            Pawn blackPawnA = new Pawn("pawn", "black", 1, 0);
            Pawn blackPawnB = new Pawn("pawn", "black", 1, 1);
            Pawn blackPawnC = new Pawn("pawn", "black", 1, 2);
            Pawn blackPawnD = new Pawn("pawn", "black", 1, 3);
            Pawn blackPawnE = new Pawn("pawn", "black", 1, 4);
            Pawn blackPawnF = new Pawn("pawn", "black", 1, 5);
            Pawn blackPawnG = new Pawn("pawn", "black", 1, 6);
            Pawn blackPawnH = new Pawn("pawn", "black", 1, 7);

            Rook blackRookA = new Rook("rook", "black", 0, 0);
            Rook blackRookH = new Rook("rook", "black", 0, 7);

            Knight blackKnightB = new Knight("knight", "black", 0, 1);
            Knight blackKnightG = new Knight("knight", "black", 0, 6);

            Bishop blackBishopC = new Bishop("bishop", "black", 0, 2);
            Bishop blackBishopF = new Bishop("bishop", "black", 0, 5);

            Queen blackQueen = new Queen("queen", "black", 0, 3);
            King blackKing = new King("king", "black", 0, 4);

            Pieces.Add(whitePawnA);
            Pieces.Add(whitePawnB);
            Pieces.Add(whitePawnC);
            Pieces.Add(whitePawnD);
            Pieces.Add(whitePawnE);
            Pieces.Add(whitePawnF);
            Pieces.Add(whitePawnG);
            Pieces.Add(whitePawnH);
            Pieces.Add(whiteRookA);
            Pieces.Add(whiteRookH);
            Pieces.Add(whiteKnightB);
            Pieces.Add(whiteKnightG);
            Pieces.Add(whiteBishopC);
            Pieces.Add(whiteBishopF);
            Pieces.Add(whiteQueen);
            Pieces.Add(whiteKing);

            Pieces.Add(blackPawnA);
            Pieces.Add(blackPawnB);
            Pieces.Add(blackPawnC);
            Pieces.Add(blackPawnD);
            Pieces.Add(blackPawnE);
            Pieces.Add(blackPawnF);
            Pieces.Add(blackPawnG);
            Pieces.Add(blackPawnH);
            Pieces.Add(blackRookA);
            Pieces.Add(blackRookH);
            Pieces.Add(blackKnightB);
            Pieces.Add(blackKnightG);
            Pieces.Add(blackBishopC);
            Pieces.Add(blackBishopF);
            Pieces.Add(blackQueen);
            Pieces.Add(blackKing);
            CreatePromotionList();

        }

        public Piece WhatPieceIsHere(Position newPosition)
        {
            foreach (Piece piece in Pieces)
            {
                if (piece.Position == newPosition)
                {
                    return piece;
                }
            }
            return null;
        }

        public string CheckForPiece(Position position)
        {
            foreach (Piece piece in Pieces)
            {
                int posRow = position.Row;
                int posCol = position.Column;
                int pieceRow = piece.Position.Row;
                int pieceCol = piece.Position.Column;

                if (pieceRow == posRow && pieceCol == posCol)
                {
                    if (piece.Covered == false)
                        return piece.Color;
                }

            }
            return "none";
        }

        public void Capture(Position position)
        {
            int posRow = position.Row;
            int posCol = position.Column;

            Pieces.RemoveAll(piece => piece.Position.Row == posRow && piece.Position.Column == posCol);

        }

        public void Capture(int row, int column)
        {
            Pieces.RemoveAll(piece => piece.Position.Row == row && piece.Position.Column == column);
        }
        
        public void NextTurn()
        {
            if (Turn == "white")
                Turn = "black";
            else
                Turn = "white";
        }

        public Position FindKingPosition()
        {
            foreach(Piece piece in Pieces)
            {
                if (piece.Name == "king" && piece.Color == Turn)
                {
                    return piece.Position;
                }
            }
            return null;
        }

        public bool CheckForMate(Board board)
        {
            foreach (Piece piece in board.Pieces)
            {
                if (piece.Color == Turn)
                {
                    foreach(Position position in Positions)
                    {
                        if (piece.CheckValidMove(board, position) == true)
                        {
                            if (piece.CheckIfMovePutsSelfInCheck(board, position) == false)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void CheckForPromotion()
        {
            foreach(Piece piece in Pieces)
            {
                if (piece.Color == Turn && piece.Name == "pawn")
                {
                    if (piece.Color == "white" && piece.Position.Row == 0)
                    {
                        Promotion = true;
                    }
                    else if (piece.Color == "black" && piece.Position.Row == 7)
                    {
                        Promotion = true;
                    }
                }
            }
        }





    }
}
