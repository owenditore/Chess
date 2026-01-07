using ChessClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace ChessClassLibrary
{
    public class FenNotation
    {
        string[,] pieceStrings = new string[8, 8];

        string fen = "";


        public FenNotation( Game game )
        {
            GenerateFEN( game );
        }


        //Methods
        /*
        public List<Piece> ReadToListOfPieces()
        {

        }
        */

        public void GenerateFEN( Game game)
        {
            GeneratePieceStrings( game );
            GenerateMainSequence();
            AddCurrentTurnColor( game );
            AddCastlingAvailability( game );
            AddEnPassantSquare( game );
            //Half move clock
            //Full move number
        }

        private string GetChessNotationColumnAsString( int columnPosition )
        {

            string chessNotationColumn = "";
            switch(columnPosition)
            {
                case 0:
                    chessNotationColumn = "a";
                    break;

                case 1:
                    chessNotationColumn = "b";
                    break;

                case 2:
                    chessNotationColumn = "c";
                    break;

                case 3:
                    chessNotationColumn = "d";
                    break;

                case 4:
                    chessNotationColumn = "e";
                    break;

                case 5:
                    chessNotationColumn = "f";
                    break;

                case 6:
                    chessNotationColumn = "g";
                    break;

                case 7:
                    chessNotationColumn = "h";
                    break;

            }
            return chessNotationColumn;
        }

        private int GetEnPassantTargetRowPosition( Turn lastTurn )
        {
            if(lastTurn.StartingPosition.Row == 1)
                return 2;
            else
                return 6;
        }

        private string GetChessNotationRowAsString( int rowPosition )
        {
            int chessNotationRow = 8 - rowPosition;
            return chessNotationRow.ToString();
        }

        private void AddEnPassantSquare( Game game )
        {
            Turn? lastTurn = game.Turns.FirstOrDefault( p =>
                p.Number == game.Turns.Count &&
                Math.Abs(p.EndingPosition.Row - p.StartingPosition.Row) == 2 &&
                p.Piece.Name == "pawn"
            );

            if(lastTurn == null)
            {
                fen = fen + " -";
                return;
            }

            int enPassantTargetRowPosition = GetEnPassantTargetRowPosition( lastTurn );

            int colPosition = lastTurn.EndingPosition.Column;
            string chessNotationColumn = GetChessNotationColumnAsString( colPosition );
            string chessNotationRow = GetChessNotationRowAsString( enPassantTargetRowPosition );

            string enPassantPositonString = chessNotationColumn + chessNotationRow;

            fen = fen + " " + enPassantPositonString;

        }



        private void AddCastlingAvailability( Game game )
        {
            string castlingRights = "";
            if(WhiteKingCanCastle( game ) == true)
            {
                if(WhiteKingRookCanCastle( game ) == true)
                {
                    castlingRights += "K";
                }
                if(WhiteQueenRookCanCastle( game ) == true)
                {
                    castlingRights += "Q";
                }
            }

            if(BlackKingCanCastle( game ) == true)
            {
                if(BlackKingRookCanCastle( game ) == true)
                {
                    castlingRights += "k";
                }
                if(BlackQueenRookCanCastle( game ) == true)
                {
                    castlingRights += "q";
                }
            }

            if(castlingRights == "")
                castlingRights = "-";

            fen = fen + " " + castlingRights;

        }

        private bool WhiteKingCanCastle( Game game )
        {
            Piece? whiteKingRook = game.WhitePlayer.Pieces.FirstOrDefault( p =>
                p.Name == "king" &&
                p.HasMoved == false &&
                p.Position.Row == 7 &&
                p.Position.Column == 4
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool BlackKingCanCastle( Game game )
        {
            Piece? whiteKingRook = game.BlackPlayer.Pieces.FirstOrDefault( p =>
                p.Name == "king" &&
                p.HasMoved == false &&
                p.Position.Row == 0 &&
                p.Position.Column == 4
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool WhiteKingRookCanCastle( Game game )
        {
            Piece? whiteKingRook = game.WhitePlayer.Pieces.FirstOrDefault( p =>
                p.Name == "rook" &&
                p.HasMoved == false &&
                p.Position.Row == 7 &&
                p.Position.Column == 7
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool WhiteQueenRookCanCastle( Game game )
        {
            Piece? whiteKingRook = game.WhitePlayer.Pieces.FirstOrDefault( p =>
                p.Name == "rook" &&
                p.HasMoved == false &&
                p.Position.Row == 7 &&
                p.Position.Column == 0
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool BlackKingRookCanCastle( Game game )
        {
            Piece? whiteKingRook = game.BlackPlayer.Pieces.FirstOrDefault( p =>
                p.Name == "rook" &&
                p.HasMoved == false &&
                p.Position.Row == 0 &&
                p.Position.Column == 7
            );
            if(whiteKingRook != null) return true;
            else return false;
        }

        private bool BlackQueenRookCanCastle( Game game )
        {
            Piece? whiteKingRook = game.BlackPlayer.Pieces.FirstOrDefault( p =>
                p.Name == "rook" &&
                p.HasMoved == false &&
                p.Position.Row == 0 &&
                p.Position.Column == 0
            );
            if(whiteKingRook != null) return true;
            else return false;
        }


        private void AddCurrentTurnColor( Game game )
        {
            string turn = game.CurrentTurnColor;
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

        public void GeneratePieceStrings( Game game )
        {
            GenerateWhitePieceStrings( game );
            GenerateBlackPieceStrings( game );
        }

        private void GenerateWhitePieceStrings( Game game )
        {
            foreach(Piece piece in game.WhitePlayer.Pieces)
            {
                string pieceChar = "";
                string pieceName = piece.Name;
                int row = piece.Position.Row;
                int col = piece.Position.Column;
                switch(pieceName)
                {
                    case "king":
                        pieceChar = "K";
                        break;

                    case "queen":
                        pieceChar = "Q";
                        break;

                    case "rook":
                        pieceChar = "R";
                        break;

                    case "bishop":
                        pieceChar = "B";
                        break;

                    case "knight":
                        pieceChar = "N";
                        break;

                    case "pawn":
                        pieceChar = "P";
                        break;
                }

                pieceStrings[row, col] = pieceChar;

            }
        }

        private void GenerateBlackPieceStrings( Game game )
        {
            foreach(Piece piece in game.WhitePlayer.Pieces)
            {
                string pieceChar = "";
                string pieceName = piece.Name;
                int row = piece.Position.Row;
                int col = piece.Position.Column;
                switch(pieceName)
                {
                    case "king":
                        pieceChar = "k";
                        break;

                    case "queen":
                        pieceChar = "q";
                        break;

                    case "rook":
                        pieceChar = "r";
                        break;

                    case "bishop":
                        pieceChar = "b";
                        break;

                    case "knight":
                        pieceChar = "n";
                        break;

                    case "pawn":
                        pieceChar = "p";
                        break;
                }

                pieceStrings[row, col] = pieceChar;

            }
        }

    }
}


