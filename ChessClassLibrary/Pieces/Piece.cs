using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessClassLibrary
{
    public class Piece
    {
        public bool HasMoved { get; set; } = false;
        public string Color { get; protected set; }

        public string OpponentColor { get; protected set; }
        public Position Position { get; set; }


        public string Name { get; protected set; }

        public bool Covered { get; protected set; } = false;


        public Piece (string name, string color, int row, int col)
        {
            Position position = new Position(row,col);
            this.Position = position;
            this.Color = color;
            this.Name = name;
            if (color == "white")
            {
                this.OpponentColor = "black";
            }
            else if (color == "black")
            {
                this.OpponentColor = "white";
            }
        }

        //Method

        public void Move(Board board, Position newPosition)
        {
            if(CheckValidMove(board,newPosition) == true)
            {
                if(CheckIfMovePutsSelfInCheck(board,newPosition) == false)
                {
                    if (board.CheckForPiece(newPosition) != "none")
                    {
                        board.Capture(newPosition);
                    }

                    if (Name == "king")
                    {
                        int horizontalMove = newPosition.Column - Position.Column;
                        if (Math.Abs(horizontalMove) == 2)
                        {
                            foreach (Piece piece in board.Pieces)
                            {
                                if (piece.Name == "rook" &&  piece.Color == Color && piece.HasMoved == false)
                                {
                                    if ((horizontalMove < 0 && piece.Position.Column < 4) || (horizontalMove > 0 && piece.Position.Column > 4))
                                    {
                                        if (piece.Position.Column < Position.Column)
                                        {
                                            piece.Position.Column = Position.Column - 1;
                                        }
                                        else
                                        {
                                            piece.Position.Column = Position.Column + 1;
                                        }
                                        piece.HasMoved = true;
                                    }
                                }
                            }
                        }
                    }

                    Position = newPosition;
                    HasMoved = true;
                    board.NextTurn();
                }
            }
        }

        public virtual bool CheckValidMove(Board board, Position newPosition)
        {
            return false;
        }

        protected int MoveCloserToZero(int number)
        {
            if (number > 0)
            {
                return number - 1;
            }
            else if (number < 0)
            {
                return number + 1;
            }
            else
                return number;
        }
        public bool CheckIfMovePutsSelfInCheck(Board board, Position newPosition)
        {

            foreach (Piece piece in board.Pieces)
            {
                if (piece.Position.Row == newPosition.Row && piece.Position.Column == newPosition.Column)
                {
                    piece.Covered = true;
                }
            }

            Position oldPosition = new Position(-1, -1);
            oldPosition.Row = Position.Row;
            oldPosition.Column = Position.Column;

            Position.Row = newPosition.Row;
            Position.Column = newPosition.Column;

            Position kingPosition = board.FindKingPosition();

            foreach (Piece piece in board.Pieces)
            {
                if (piece.Position.Row == Position.Row && piece.Position.Column == Position.Column)
                {

                }
                else if (piece.CheckValidMove(board, kingPosition))
                {
                    Position.Row = oldPosition.Row;
                    Position.Column = oldPosition.Column;
                    foreach (Piece piece2 in board.Pieces)
                    {
                        if (piece2.Covered == true)
                            piece2.Covered = false;
                    }
                    return true;
                }
            }

            Position.Row = oldPosition.Row;
            Position.Column = oldPosition.Column;
            foreach (Piece piece2 in board.Pieces)
            {
                if (piece2.Covered == true)
                    piece2.Covered = false;
            }
            return false;
        }

    }
}
