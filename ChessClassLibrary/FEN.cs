using ChessClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class FenNotation
    {
        string[,] pieceStrings = new string[8, 8];

        string fen = "";


        public FenNotation( Board board )
        {
            GenerateFEN( board );
        }


        //Methods
        public void GenerateFEN( Board board )
        {
            GeneratePieceStrings( board );
            GenerateMainSequence();
            AddCurrentTurn( board );
            AddCastlingAvailability( board );
            //AddEnPassantSquare(board);
            //Half move clock
            //Full move number
        }


        private void AddEnPassantSquare( Board board )
        {
            Move? lastMove = board.Moves.FirstOrDefault( p =>
                p.Number == board.Moves.Count &&
                p.EndingPosition.Row != p.CapturedPiece.Position.Row &&
                p.EndingPosition.Column != p.CapturedPiece.Position.Column &&
                p.CapturedPiece != null
            );
            if(lastMove == null)
            {
                fen = fen + " -";
            }
            else
            {
                int colPosition = lastMove.EndingPosition.Column;
                string colPositionString = "";
                switch(colPosition)
                {
                    case 0:
                        colPositionString = "a";
                        break;

                    case 1:
                        colPositionString = "b";
                        break;

                    case 2:
                        colPositionString = "c";
                        break;

                    case 3:
                        colPositionString = "d";
                        break;

                    case 4:
                        colPositionString = "e";
                        break;

                    case 5:
                        colPositionString = "f";
                        break;

                    case 6:
                        colPositionString = "g";
                        break;

                    case 7:
                        colPositionString = "h";
                        break;

                }

                int rowPosition = Math.Abs( lastMove.EndingPosition.Row - lastMove.StartingPosition.Row );
                string rowPositionString = rowPosition.ToString();
                string enPassantPositonString = colPositionString + rowPositionString;

                fen = fen + " " + enPassantPositonString;
            }
        }



        private void AddCastlingAvailability( Board board )
        {
            string castlingRights = "";
            if(WhiteKingCanCastle( board ) == true)
            {
                if(WhiteKingRookCanCastle( board ) == true)
                {
                    castlingRights += "K";
                }
                if(WhiteQueenRookCanCastle( board ) == true)
                {
                    castlingRights += "Q";
                }
            }

            if(BlackKingCanCastle( board ) == true)
            {
                if(BlackKingRookCanCastle( board ) == true)
                {
                    castlingRights += "k";
                }
                if(BlackQueenRookCanCastle( board ) == true)
                {
                    castlingRights += "q";
                }
            }

            if(castlingRights == "")
                castlingRights = "-";

            fen = fen + " " + castlingRights;

        }

        private bool WhiteKingCanCastle( Board board )
        {
            Piece? whiteKingRook = board.Pieces.FirstOrDefault( p =>
                p.Color == "white" &&
                p.Name == "king" &&
                p.HasMoved == false &&
                p.Position.Row == 7 &&
                p.Position.Column == 4
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool BlackKingCanCastle( Board board )
        {
            Piece? whiteKingRook = board.Pieces.FirstOrDefault( p =>
                p.Color == "black" &&
                p.Name == "king" &&
                p.HasMoved == false &&
                p.Position.Row == 0 &&
                p.Position.Column == 4
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool WhiteKingRookCanCastle( Board board )
        {
            Piece? whiteKingRook = board.Pieces.FirstOrDefault( p =>
                p.Color == "white" &&
                p.Name == "rook" &&
                p.HasMoved == false &&
                p.Position.Row == 7 &&
                p.Position.Column == 7
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool WhiteQueenRookCanCastle( Board board )
        {
            Piece? whiteKingRook = board.Pieces.FirstOrDefault( p =>
                p.Color == "white" &&
                p.Name == "rook" &&
                p.HasMoved == false &&
                p.Position.Row == 7 &&
                p.Position.Column == 0
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool BlackKingRookCanCastle( Board board )
        {
            Piece? whiteKingRook = board.Pieces.FirstOrDefault( p =>
                p.Color == "black" &&
                p.Name == "rook" &&
                p.HasMoved == false &&
                p.Position.Row == 0 &&
                p.Position.Column == 7
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool BlackQueenRookCanCastle( Board board )
        {
            Piece? whiteKingRook = board.Pieces.FirstOrDefault( p =>
                p.Color == "black" &&
                p.Name == "rook" &&
                p.HasMoved == false &&
                p.Position.Row == 0 &&
                p.Position.Column == 0
            );
            if(whiteKingRook != null) return true;
            else return false;
        }


        private void AddCurrentTurn( Board board )
        {
            string turn = board.Turn;
            string turnLetter = "";
            if(turn == "white")
                turnLetter = "w";
            else
                turnLetter = "b";

            fen = fen + " " + turnLetter;
        }

        public void GenerateMainSequence()
        {
            for(int i = 0; i < 8; i++)
            {
                int thisRowEmptySpacesInARow = 0;
                for(int j = 0; j < 8; j++)
                {
                    if(pieceStrings[i, j] == null)
                    {
                        thisRowEmptySpacesInARow++;
                    }

                    else
                    {
                        AddEmptySpaceNumber( thisRowEmptySpacesInARow );
                        thisRowEmptySpacesInARow = 0;
                        fen += pieceStrings[i, j];
                    }

                    if(j == 7)
                    {
                        AddEmptySpaceNumber( thisRowEmptySpacesInARow );
                        thisRowEmptySpacesInARow = 0;
                        if(i != 7)
                            fen += "/";
                    }

                }
            }
        }

        private void AddEmptySpaceNumber( int spaces )
        {
            if(spaces == 0)
                return;
            string spacesString = spaces.ToString();
            fen += spacesString;
        }

        public void GeneratePieceStrings( Board board )
        {
            foreach(Piece piece in board.Pieces)
            {
                string pieceChar = "";
                string pieceName = piece.Name;
                string pieceColor = piece.Color;
                int row = piece.Position.Row;
                int col = piece.Position.Column;
                switch(pieceName)
                {
                    case "king":
                        if(piece.Color == "white") pieceChar = "K";
                        else pieceChar = "k";
                        break;

                    case "queen":
                        if(piece.Color == "white") pieceChar = "Q";
                        else pieceChar = "q";
                        break;

                    case "rook":
                        if(piece.Color == "white") pieceChar = "R";
                        else pieceChar = "r";
                        break;

                    case "bishop":
                        if(piece.Color == "white") pieceChar = "B";
                        else pieceChar = "b";
                        break;

                    case "knight":
                        if(piece.Color == "white") pieceChar = "N";
                        else pieceChar = "n";
                        break;

                    case "pawn":
                        if(piece.Color == "white") pieceChar = "P";
                        else pieceChar = "p";
                        break;
                }

                pieceStrings[row, col] = pieceChar;

            }
        }
    }
}


