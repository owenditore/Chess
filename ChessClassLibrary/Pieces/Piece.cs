using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Piece
    {
        public bool HasMoved { get; set; } = false;
        public string Color { get; protected set; }
        public Position Position { get; protected set; }

        public string Name { get; protected set; }


        public Piece (string name, string color, int row, int col)
        {
            Position position = new Position(row,col);
            this.Position = position;
            this.Color = color;
            this.Name = name;
        }

        //Method

        public void Move(Board board,Position newPosition)
        {
            if(CheckValidMove(board,newPosition))
            {
                Position = newPosition;
            }
            else
            {
                Console.WriteLine("Invalid Move.");
            }
            HasMoved = true;
            Board.RefreshDisplay();

        }

        protected virtual bool CheckValidMove(Board board, Position newPosition)
        {
            return true;
        }

    }
}
