using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Square
    {

        public Position Position { get; set; } = new Position( -1, -1 );

        public Piece? Piece { get; set; }


        public Square( int row, int column )
        {
            this.Position.Row = row;
            this.Position.Column = column;
        }


    }
}
