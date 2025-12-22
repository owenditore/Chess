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

        public override bool CheckValidMove(Board board, Position newPosition)
        {
            Position intermediaryPosition = new Position(-1, -1);
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
                        intermediaryPosition.Row = Position.Row - 1;
                        intermediaryPosition.Column = Position.Column;
                        if (board.CheckForPiece(intermediaryPosition) == "none")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    //Capture
                    else if (verticalMove == -1 && Math.Abs(horizontalMove) == 1 && stateOfNewPosition == "black")
                    {
                        return true;
                    }

                    //En Passant ??

                    else if (verticalMove == -1 && Math.Abs(horizontalMove) == 1 && stateOfNewPosition == "none")
                    {
                        int enPassantRow = newPosition.Row + 1;
                        int enPassantCol = newPosition.Column;
                        int enPassantStartingRow = enPassantRow - 2;
                        int enPassantStartingCol = enPassantCol;

                        Position enPassantPosition = new Position(enPassantRow, enPassantCol);
                        if (board.CheckForPiece(enPassantPosition) == "black"  && board.NameOfPiece(enPassantPosition) == "pawn")
                        {
                            Move lastMove = board.ReturnLastMove(board);

                            if (lastMove.Piece.Name == "pawn" && lastMove.Piece.Color == "black" && lastMove.EndingPosition.Row == enPassantRow && lastMove.EndingPosition.Column == enPassantCol)
                            {
                                if (lastMove.StartingPosition.Row == enPassantStartingRow && lastMove.StartingPosition.Column == enPassantStartingCol)
                                {
                                    return true;
                                }
                            }

                        }
                    }

                    return false;


                case "black":
                    //Normal Move
                    if (verticalMove == 1 && horizontalMove == 0 && stateOfNewPosition == "none")
                        return true;

                    //First Move
                    else if (verticalMove == 2 && horizontalMove == 0 && stateOfNewPosition == "none" && HasMoved == false)
                    {
                        intermediaryPosition.Row = Position.Row + 1;
                        intermediaryPosition.Column = Position.Column;
                        if (board.CheckForPiece(intermediaryPosition) == "none")
                            return true;
                        else
                            return false;
                    }

                    //Capture
                    else if (verticalMove == 1 && Math.Abs(horizontalMove) == 1 && stateOfNewPosition == "white")
                    {
                        return true;
                    }

                    //En Passant?

                    else if (verticalMove == 1 && Math.Abs(horizontalMove) == 1 && stateOfNewPosition == "none")
                    {
                        int enPassantRow = newPosition.Row - 1;
                        int enPassantCol = newPosition.Column;
                        int enPassantStartingRow = enPassantRow + 2;
                        int enPassantStartingCol = enPassantCol;

                        Position enPassantPosition = new Position(enPassantRow, enPassantCol);
                        if (board.CheckForPiece(enPassantPosition) == "white" && board.NameOfPiece(enPassantPosition) == "pawn")
                        {
                            Move lastMove = board.ReturnLastMove(board);

                            if (lastMove.Piece.Name == "pawn" && lastMove.Piece.Color == "white" && lastMove.EndingPosition.Row == enPassantRow && lastMove.EndingPosition.Column == enPassantCol)
                            {
                                if (lastMove.StartingPosition.Row == enPassantStartingRow && lastMove.StartingPosition.Column == enPassantStartingCol)
                                {
                                    return true;
                                }
                            }

                        }
                    }


                    return false;

            }
            return false;
        }


    }
}
