using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChessClassLibrary
{
    public class Board
    {
        public List<Piece> Pieces { get; set; } = new List<Piece>();
        public bool Check { get; set; } = false;
        public bool CheckMate { get; set; } = false;
        

        public void TestSetup()
        {
            Pawn testPawn = new Pawn("whitePawnE","white", 4, 4);
            Pieces.Add(testPawn);
        }
        public void SetupGame()
        {
            Rook whiteRookA = new Rook("whiteRookA","white", 7, 0);
            Pieces.Add(whiteRookA);
            Knight whiteKnightB = new Knight("whiteKinghtB","white", 7, 1);
            Pieces.Add(whiteKnightB);
            King whiteKing = new King("whiteKing","white", 7, 4);
            //Finish filling this up.
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
            return;
        }

        public string CheckForPiece(Position newPosition)
        {
            foreach (Piece piece in Pieces)
            {
                if (piece.Position == newPosition)
                {
                    return piece.Color;
                }
            }
            return "none";
        }

        public static void RefreshDisplay()
        {
            //MainWindow.RefreshDisplay(pieces);
        }

    }
}
