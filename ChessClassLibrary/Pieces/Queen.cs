using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Queen : Piece
    {

        //Constructor
        public Queen(string color, int row, int col) : base(color, row, col)
        {

        }

        //Methods

        protected override bool CheckValidMove(Position newPosition)
        {
            return true;
        }

    }
}
