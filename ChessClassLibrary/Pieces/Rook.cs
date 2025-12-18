using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Rook : Piece
    {

        //Constructor
        public Rook(string name, string color, int row, int col) : base(name, color, row, col)
        {

        }

        //Methods

        protected override bool CheckValidMove(Position newPosition)
        {
            return true;
        }

    }
}
