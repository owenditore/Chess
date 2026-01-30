using ChessClassLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class FenNotation
    {
        string[,] pieceStrings = new string[8, 8];

        public string Fen { get; set; } = "";

        private string MainSequence { get; set; }

        private string CurrentTurnColor { get; set; }

        private string CastlingAvailability { get; set; }

        private string EnPassantSquare { get; set; }

        private string HalfMoveClock { get; set; }

        private string FullMoveNumber { get; set; }



        public FenNotation( Board board )
        {
            GenerateFEN( board );
        }

        public FenNotation( string fen )
        {
            this.Fen = fen;
            string[] partsOfFen = fen.Split( ' ' );
            this.MainSequence = partsOfFen[0];
            this.CurrentTurnColor = partsOfFen[1];
            this.CastlingAvailability = partsOfFen[2];
            this.EnPassantSquare = partsOfFen[3];
            this.HalfMoveClock = partsOfFen[4];
            this.FullMoveNumber = partsOfFen[5];
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
            AssembleFen();
        }

        private void AssembleFen()
        {
            Fen = MainSequence + " " + CurrentTurnColor + " " + CastlingAvailability + " " + EnPassantSquare + " " + HalfMoveClock + " " + FullMoveNumber;
        }

        private void AddFullMoveNumber( Board board )
        {
            FullMoveNumber = board.FullMoveNumber.ToString();
        }

        private void AddHalfMoveClock( Board board )
        {
            HalfMoveClock = board.HalfMoveClock.ToString();
        }

        private void AddEnPassantSquare( Board board )
        {
            Turn? lastTurn = board.LastTurn;

            if(lastTurn == null)
            {
                EnPassantSquare = "-";
                return;
            }

            if(lastTurn.Piece.Name != "pawn")
            {
                EnPassantSquare = "-";
                return;
            }


            if(Math.Abs(lastTurn.EndingPosition.Row - lastTurn.StartingPosition.Row) != 2)
            {
                EnPassantSquare = "-";
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

            EnPassantSquare = enPassantPositonString;

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

            CastlingAvailability = castlingRights;

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

            CurrentTurnColor = turnLetter;
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
                        MainSequence += pieceStrings[i, j];
                    }

                    if(j == 7)
                    {
                        AddEmptySpaceNumber( thisRowEmptySpacesInARow );
                        thisRowEmptySpacesInARow = 0;
                        if(i != 7)
                            MainSequence += "/";
                    }

                }
            }
        }

        private void AddEmptySpaceNumber( int spaces )
        {
            if(spaces == 0)
                return;
            string spacesString = spaces.ToString();
            MainSequence += spacesString;
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

        public List<Piece> GetListOfPieces()
        {
            int currentRow = 0;
            int currentColumn = 0;
            int currentInt;

            List <Piece> pieces = new List<Piece>();

            foreach (char character in MainSequence)
            {
                string current = character.ToString();

                if (current == "/")
                {
                    currentRow++;
                    currentColumn = 0;
                    continue;
                }

                if(int.TryParse(current, out currentInt))
                {
                    currentColumn += currentInt;
                    continue;
                }

                Piece currentPiece = GetPieceFromMainSequenceCharacter( current, currentRow, currentColumn);

                pieces.Add( currentPiece );

                currentColumn++;
            }

            return pieces;
        }

        private Piece GetPieceFromMainSequenceCharacter( string letter, int row, int column )
        {
            bool canCastle;

            switch(letter)
            {
                case "P":

                    if(row == 6)
                        return new Pawn( "pawn", "white", row, column, false );

                    else
                        return new Pawn( "pawn", "white", row, column, true );

                case "p":

                    if(row == 1)
                        return new Pawn( "pawn", "black", row, column, false );

                    else
                        return new Pawn( "pawn", "black", row, column, true );

                case "N":
                    return new Knight( "knight", "white", row, column );
                case "n":
                    return new Knight( "knight", "black", row, column );
                case "B":
                    return new Bishop( "bishop", "white", row, column );
                case "b":
                    return new Bishop( "bishop", "black", row, column );
                case "R":
                    canCastle = AssessRookCastlingRights( letter, row, column );
                    return new Rook( "rook", "white", row, column, !canCastle );
                case "r":
                    canCastle = AssessRookCastlingRights( letter, row, column );
                    return new Rook( "rook", "black", row, column, !canCastle );
                case "Q":
                    return new Queen( "queen", "white", row, column );
                case "q":
                    return new Queen( "queen", "black", row, column );
                case "K":
                    canCastle = AssessKingCastlingRights( letter, row, column );
                    return new King( "king", "white", row, column, !canCastle );
                case "k":
                    canCastle = AssessKingCastlingRights( letter, row, column );
                    return new King( "king", "black", row, column, !canCastle );
            }
            return null;
        }

        private bool AssessKingCastlingRights( string letter, int row, int column )
        {
            if(letter == "K")
            {
                if(CastlingAvailability.Contains( "K" ) || CastlingAvailability.Contains( "Q" ))
                    return true;
            }
            if(letter == "k")
            {
                if(CastlingAvailability.Contains( "k" ) || CastlingAvailability.Contains( "q" ))
                    return true;
            }
            return false;
        }

        private bool AssessRookCastlingRights( string letter, int row, int column )
        {
            if(letter == "R")
            {
                if(column == 0)
                {
                    return CastlingAvailability.Contains( "Q" );
                }
                if(column == 7)
                {
                    return CastlingAvailability.Contains( "K" );
                }
            }

            if(letter == "r")
            {
                if(column == 0)
                {
                    return CastlingAvailability.Contains( "q" );
                }
                if(column == 7)
                {
                    return CastlingAvailability.Contains( "k" );
                }
            }
            return false;
        }

        public Turn GetLastTurnForEnPassant()
        {
            if(EnPassantSquare == "-")
            {
                return null;
            }

            string fenColumnLetter = EnPassantSquare[0].ToString();
            string fenRowNumber = EnPassantSquare[1].ToString();

            int EnPassantSquareColumn = ConvertFenColumnLetterToRegularColumnNumber( fenColumnLetter );
            int EnPassantSquareRow = ConvertFenRowNumberToRegularRowNumber( fenRowNumber );

            if (CurrentTurnColor == "w")
            {
                Position startingPosition = new Position( (EnPassantSquareRow - 1) , EnPassantSquareColumn );
                Position endingPosition = new Position( ( EnPassantSquareRow + 1 ), EnPassantSquareColumn );
                Pawn pawnThatMoved = new Pawn( "pawn", "black", endingPosition.Row, endingPosition.Column );
                Turn lastTurn = new Turn( pawnThatMoved, startingPosition, endingPosition );
                return lastTurn;
            }
            else
            {
                Position startingPosition = new Position( ( EnPassantSquareRow + 1 ), EnPassantSquareColumn );
                Position endingPosition = new Position( ( EnPassantSquareRow - 1 ), EnPassantSquareColumn );
                Pawn pawnThatMoved = new Pawn( "pawn", "white", endingPosition.Row, endingPosition.Column );
                Turn lastTurn = new Turn( pawnThatMoved, startingPosition, endingPosition );
                return lastTurn;
            }

        }

        private int ConvertFenColumnLetterToRegularColumnNumber(string columnCharacter)
        {
            switch (columnCharacter)
            {
                case "a": return 0;
                
                case "b": return 1;

                case "c": return 2;

                case "d": return 3;

                case "e": return 4;

                case "f": return 5;

                case "g": return 6;

                case "h": return 7;
            }
            return -1;
        }

        private int ConvertFenRowNumberToRegularRowNumber(string rowCharacter)
        {
            int initalRowNumber = int.Parse(rowCharacter);
            return (8 -  initalRowNumber);
        }

        public string GetCurrentTurnColor()
        {
            if(CurrentTurnColor == "w")
            {
                return "white";
            }
            else
            {
                return "black";
            }
        }

        public int GetHalfMoveClock()
        {
            return int.Parse( HalfMoveClock );
        }

        public int GetFullMoveNumber()
        {
            return int.Parse( FullMoveNumber );
        }

    }
}


