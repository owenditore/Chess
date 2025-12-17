using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Board
    {
        List<Piece> pieces = new List<Piece>();
        //Method

        public void TestSetup()
        {
            Pawn testPawn = new Pawn("white", 4, 4);
            pieces.Add(testPawn);
        }
        public void SetupGame()
        {
            Rook whiteRookA = new Rook("white", 7, 0);
            pieces.Add(whiteRookA);
            Knight whiteKnightB = new Knight("white", 7, 1);
            pieces.Add(whiteKnightB);
            King whiteKing = new King("white", 7, 4);
            //Finish filling this up.
        }

        public static void RefreshDisplay()
        {
            foreach(Piece piece in pieces)
            {
                //Read Position
                //Convert from numbers to place on the board
                //Display the icon of the Piece there
            }
        }
    }
}
