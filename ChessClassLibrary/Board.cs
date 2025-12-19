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

        public string Turn { get; set; } = "white";
        

        public void TestSetup()
        {
            Pawn testPawn1 = new Pawn("pawn","white", 3, 3);
            Bishop testBishop1 = new Bishop("bishop", "white", 5, 5);
            Pieces.Add(testPawn1);
            Pieces.Add(testBishop1);
        }

        public void AddPiece(Piece piece)
        {
            Pieces.Add(piece); 
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
        
        public void NextTurn()
        {
            if (Turn == "white")
                Turn = "black";
            else
                Turn = "white";
        }

    }
}
