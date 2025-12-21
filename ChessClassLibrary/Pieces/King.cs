using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class King : Piece
    {

        //Constructor
        public King(string name, string color, int row, int col) : base(name, color, row, col)
        {

        }

        //Methods

        public override bool CheckValidMove(Board board, Position newPosition)
        {
            Position intermediaryPosition = new Position(-1, -1);
            string stateOfNewPosition = board.CheckForPiece(newPosition); //white,black,none
            int verticalMove = newPosition.Row - Position.Row;
            int horizontalMove = newPosition.Column - Position.Column;

            //No friendly piece at new position
            if (stateOfNewPosition == Color)
            {
                return false;
            }

            //Can Move 1 space away in any direction
            else if (Math.Abs(verticalMove) <= 1 && Math.Abs(horizontalMove) <= 1)
            {
                return true;
            }

            //Castling
            else if (HasMoved == false)
            {
                if (Math.Abs(horizontalMove) == 2 && verticalMove ==0)
                {
                    foreach(Piece piece in board.Pieces)
                    {
                        if (piece.Name == "rook" && piece.Color == Color)
                        {
                            int castleRookRowPosition = Position.Row;
                            int castleRookColumnPosition = -1;
                            if (horizontalMove > 0)
                            {
                                castleRookColumnPosition = Position.Column + 3;
                            }
                            else if (horizontalMove < 0)
                            {
                                castleRookColumnPosition = Position.Column - 4; ;
                            }

                            if (piece.Position.Row ==  castleRookRowPosition && piece.Position.Column == castleRookColumnPosition)
                            {
                                if (piece.HasMoved == false)
                                {
                                    do
                                    {
                                        if (castleRookColumnPosition < 4)
                                            castleRookColumnPosition++;
                                        else if (castleRookColumnPosition > 4)
                                            castleRookColumnPosition--;

                                        intermediaryPosition.Column = castleRookColumnPosition;
                                        intermediaryPosition.Row = castleRookRowPosition;
                                        string stateOfIntermediaryPosition = board.CheckForPiece(intermediaryPosition);
                                        if (stateOfIntermediaryPosition != "none")
                                        {
                                            return false;
                                        }
                                    } while (Math.Abs(Position.Column - castleRookColumnPosition) != 1);
                                    return true;
                                }
                            }


                        }
                    }
                }
            }

            else
            { 
                return false; 
            }
            return false;

        }

    }
}
