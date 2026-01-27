using ChessClassLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class FenNotation
    {
        string[,] pieceStrings = new string[8, 8];

        public string fen { get; set; } = "";


        public FenNotation( Board board )
        {
            GenerateFEN( board );
        }


        //Methods
        public void GenerateFEN( Board board )
        {
            GeneratePieceStrings( board );
            GenerateMainSequence();
            AddCurrentTurnColor( board );
            AddCastlingAvailability( board );
            AddEnPassantSquare(board);
            AddHalfMoveClock( board );
            AddFullMoveNumber( board );
        }

        private void AddFullMoveNumber( Board board )
        {
            string fullMoveNumber = board.FullMoveNumber.ToString();

            fen = fen + " " + fullMoveNumber;
        }

        private void AddHalfMoveClock( Board board )
        {
            string halfMoveClock = board.HalfMoveClock.ToString();

            fen = fen + " " + halfMoveClock;
        }

        private void AddEnPassantSquare( Board board )
        {
            Turn? lastTurn = board.Turns.FirstOrDefault( p =>
                p.Number == board.Turns.Count &&
                p.Piece.Name == "pawn"
            );

            if(lastTurn == null)
            {
                fen = fen + " -";
                return;
            }

            if(Math.Abs(lastTurn.EndingPosition.Row - lastTurn.StartingPosition.Row) != 2)
            {
                fen = fen + " -";
                return;
            }

            int colPosition = lastTurn.EndingPosition.Column;
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

            int rowPosition = 0;

            if(lastTurn.Piece.Color == "white")
            {
                rowPosition = 8 - ( lastTurn.EndingPosition.Row + 1 );
            }
            else
            {
                rowPosition = 8 - ( lastTurn.EndingPosition.Row - 1 );
            }

            string rowPositionString = rowPosition.ToString();
            string enPassantPositonString = colPositionString + rowPositionString;

            fen = fen + " " + enPassantPositonString;

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


        private void AddCurrentTurnColor( Board board )
        {
            string turn = board.CurrentTurnColor;
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


