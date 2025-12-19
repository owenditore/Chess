using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Queen : Piece
    {

        //Constructor
        public Queen(string name, string color, int row, int col) : base(name, color, row, col)
        {

        }

        //Methods

        protected override bool CheckValidMove(Board board, Position newPosition)
        {
            return true;
        }

    }
}
