using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Move
    {
        //Properties
        public Position StartingPosition { get; set; }

        public Position EndingPosition { get; set; }

        public Piece Piece { get; set; }

        public Piece CapturedPiece { get; set; }

        public string Notation { get; set; }

        public string Number {  get; set; }


        //Constructors
        public Move(string number, Piece movedPiece, Piece capturedPiece, int startRow, int startColumn, int endRow, int endColumn)
        {
            this.Number = number;
            this.StartingPosition = new Position(startRow, startColumn);
            this.EndingPosition = new Position(endRow, endColumn);
            this.Piece = movedPiece;
            this.CapturedPiece = capturedPiece;
            GenerateNotation();
        }
        public Move(Piece movedPiece, Piece capturedPiece, int startRow, int startColumn, int endRow, int endColumn)
        {
            this.StartingPosition = new Position(startRow, startColumn);
            this.EndingPosition = new Position(endRow, endColumn);
            this.Piece = movedPiece;
            this.CapturedPiece = capturedPiece;
            GenerateNotation();
        }

        public Move(string number, Piece movedPiece, int startRow, int startColumn, int endRow, int endColumn)
        {
            this.Number = number;
            this.StartingPosition = new Position(startRow, startColumn);
            this.EndingPosition = new Position(endRow, endColumn);
            this.Piece = movedPiece;
            GenerateNotation();
        }

        public Move(Piece movedPiece, int startRow, int startColumn, int endRow, int endColumn)
        {
            this.StartingPosition = new Position(startRow, startColumn);
            this.EndingPosition = new Position(endRow, endColumn);
            this.Piece = movedPiece;
            GenerateNotation();
        }

        public Move(Piece piece, Piece capturedPiece, Position startingPosition, Position endingPosition)
        {
            this.StartingPosition = new Position(startingPosition.Row, startingPosition.Column);
            this.EndingPosition = new Position(endingPosition.Row, endingPosition.Column);
            this.Piece = piece;
            this.CapturedPiece = capturedPiece;
            GenerateNotation();
        }

        public Move(Piece piece, Position startingPosition, Position endingPosition)
        {
            this.StartingPosition = new Position(startingPosition.Row, startingPosition.Column);
            this.EndingPosition = new Position(endingPosition.Row, endingPosition.Column);
            this.Piece = piece;
            GenerateNotation();
        }

        public void GenerateNotation()
        {

        }
    }
}
