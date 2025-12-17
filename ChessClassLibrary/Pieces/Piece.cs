using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Piece
    {
        public string Color { get; protected set; }
        public Position Position { get; protected set; }


        public Piece (string color, int row, int col)
        {
            Position position = new Position(row,col);
            this.Position = position;
            this.Color = color;
        }

        //Method

        public void Move(Position newPosition)
        {
            if(CheckValidMove(newPosition))
            {
                Position = newPosition;
            }
            else
            {
                Console.WriteLine("Invalid Move.");
            }
            Board.RefreshDisplay();

        }

        protected virtual bool CheckValidMove(Position newPosition)
        {
            return true;
        }

    }
}
