using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChessClassLibrary
{
    public class Pawn : Piece
    {

        //Constructor
        public Pawn(string name, string color, int row, int col) : base(name, color, row, col)
        {

        }

        //Methods

        protected override bool CheckValidMove(Board board, Position newPosition)
        {
            Position intermediaryPosition = newPosition;
            string stateOfNewPosition = board.CheckForPiece(newPosition); //white,black,none
            int verticalMove = newPosition.Row - Position.Row;
            int horizontalMove = newPosition.Column - Position.Column;

            switch (Color)
            {
                case "white":
                    //Normal Move
                    if (verticalMove == -1 && horizontalMove == 0 && stateOfNewPosition == "none")
                        return true;

                    //First Move 
                    else if (verticalMove == -2 && horizontalMove == 0 && stateOfNewPosition == "none" && HasMoved == false)
                    {
                        intermediaryPosition.Row += 1;
                        if (board.CheckForPiece(intermediaryPosition) == "none")
                        {
                            intermediaryPosition.Row -= 1;
                            return true;
                        }
                        else
                        {
                            intermediaryPosition.Row -= 1;
                            return false;
                        }
                    }

                    //Capture
                    else if (verticalMove == -1 && Math.Abs(horizontalMove) == 1 && stateOfNewPosition == "black")
                    {
                        board.Capture(newPosition);
                        return true;
                    }

                    else
                        return false;

                    //En Passant ??

                case "black":
                    //Normal Move
                    if (verticalMove == 1 && horizontalMove == 0 && stateOfNewPosition == "none")
                        return true;

                    //First Move
                    else if (verticalMove == 2 && horizontalMove == 0 && stateOfNewPosition == "none" && HasMoved == false)
                    {
                        intermediaryPosition.Row += 1;
                        if (board.CheckForPiece(intermediaryPosition) == "none")
                            return true;
                        else
                            return false;
                    }

                    //Capture
                    else if (verticalMove == 1 && Math.Abs(horizontalMove) == 1 && stateOfNewPosition == "white")
                    {
                        board.Capture(newPosition);
                        return true;
                    }

                    //En Passant?

                    else
                        return false;

            }
            return false;
        }

    }
}
