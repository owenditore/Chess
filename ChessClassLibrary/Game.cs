using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace ChessClassLibrary
{
    public class Game
    {

        public Board Board { get; set; }

        public Player WhitePlayer { get; set; }

        public Player BlackPlayer { get; set; }

        public List<FenNotation> FENs { get; set; } = new List<FenNotation>();

        public List<Turn> Turns { get; set; } = new List<Turn>();

        public string CurrentTurnColor { get; set; } = "white";

        public Player CurrentPlayer {  get; set; }

        public Player NotCurrentPlayer { get; set; }

        public List<Piece> PromotionList { get; set; } = new List<Piece>();

        public Game()
        {
            WhitePlayer = new Player( "white" );
            BlackPlayer = new Player( "black" );

            CurrentPlayer = WhitePlayer;
            NotCurrentPlayer = BlackPlayer;

            CreatePromotionList();

            this.Board = new Board( this );
        }

        public void SetupGame()
        {
            WhitePlayer.SetupStartingPosition();
            BlackPlayer.SetupStartingPosition();
            this.Board.AddPiecesToBoard();
        }

        public void PromotePiece( Piece promotablePiece, Piece promoteToPiece )
        {
            switch(promoteToPiece.Name)
            {
                case "queen":
                    this.CurrentPlayer.Pieces.Add( new Queen( promoteToPiece.Name, promoteToPiece.Color, promotablePiece.Position.Row, promotablePiece.Position.Column ) );
                    break;
                case "rook":
                    this.CurrentPlayer.Pieces.Add( new Rook( promoteToPiece.Name, promoteToPiece.Color, promotablePiece.Position.Row, promotablePiece.Position.Column ) );
                    break;
                case "bishop":
                    this.CurrentPlayer.Pieces.Add( new Bishop( promoteToPiece.Name, promoteToPiece.Color, promotablePiece.Position.Row, promotablePiece.Position.Column ) );
                    break;
                case "knight":
                    this.CurrentPlayer.Pieces.Add( new Knight( promoteToPiece.Name, promoteToPiece.Color, promotablePiece.Position.Row, promotablePiece.Position.Column ) );
                    break;
                default:
                    break;
            }

            this.Capture( promotablePiece );
        }


        public void AddFEN()
        {
            FENs.Add( new FenNotation( this ) );
        }

        public void GenerateGameStateFromFEN()
        {

        }

        public void Capture( Position newPosition )
        {
            this.NotCurrentPlayer.Capture( newPosition );
            this.Board.Capture( newPosition );
        }

        public void Capture( Piece piece )
        {
            this.NotCurrentPlayer.Capture( piece );
            this.Board.Capture( piece );
        }

        public Turn ReturnLastTurn()
        {
            int numberOfTurns = this.Turns.Count();

            return this.Turns.FirstOrDefault( t => t.Number == numberOfTurns );

        }

        public void NextTurn()
        {
            CurrentTurnColor = ( CurrentTurnColor == "white" ) ? "black" : "white";

            CurrentPlayer = ( CurrentPlayer == WhitePlayer ) ? BlackPlayer : WhitePlayer;

            NotCurrentPlayer = ( NotCurrentPlayer == WhitePlayer ) ? BlackPlayer : WhitePlayer;
        }

        public string CheckForMateOrDraw()
        {
            string checkForMate = this.CheckForCheckOrStaleMate();
            if(checkForMate == "checkmate") return "checkmate";
            if(checkForMate == "stalemate") return "draw";

            string checkForInsufficientMaterial = this.CheckForInsufficientMaterial();
            if(checkForInsufficientMaterial == "draw") return "draw";

            //string checkForThreeRepitition = CheckForThreeRepitition(board);
            //if (checkForThreeRepitition == "threerepitition") return checkForThreeRepitition;

            //string checkForFiftyMoves = CheckForFiftyMoves(board);
            //if (checkForFiftyMoves == "fiftymoves") return checkForFiftyMoves;

            return "";
        }

        public string CheckForInsufficientMaterial()
        {
            if(WhitePlayer.CheckForInsufficientMaterial() == false)
            {
                return "";
            }
            if(BlackPlayer.CheckForInsufficientMaterial() == false)
            {
                return "";
            }

            return "draw";
        }

        public string CheckForCheckOrStaleMate()
        {
            if(this.CurrentPlayer.AnyPieceAllowedToMove( this.Board ))
            {
                return "";
            }

            if(this.Board.IsTheKingInCheck())
            {
                return "checkmate";
            }

            return "stalemate";
        }

        public Piece CheckForPieceToPromoteTo( Position clickedPosition )
        {
            Piece? pieceToPromoteTo = this.PromotionList.FirstOrDefault( p =>
                p.Position.IsEqual( clickedPosition ) &&
                p.Color == this.CurrentTurnColor
            );

            return pieceToPromoteTo;
        }

        public void CreatePromotionList()
        {
            Queen whitePromoteQueen = new Queen( "queen", "white", 2, 7 );
            Rook whitePromoteRook = new Rook( "rook", "white", 3, 7 );
            Bishop whitePromoteBishop = new Bishop( "bishop", "white", 4, 7 );
            Knight whitePromoteKnight = new Knight( "knight", "white", 5, 7 );

            Queen blackPromoteQueen = new Queen( "queen", "black", 2, 0 );
            Rook blackPromoteRook = new Rook( "rook", "black", 3, 0 );
            Bishop blackPromoteBishop = new Bishop( "bishop", "black", 4, 0 );
            Knight blackPromoteKnight = new Knight( "knight", "black", 5, 0 );

            PromotionList.Add( whitePromoteQueen );
            PromotionList.Add( whitePromoteRook );
            PromotionList.Add( whitePromoteBishop );
            PromotionList.Add( whitePromoteKnight );

            PromotionList.Add( blackPromoteQueen );
            PromotionList.Add( blackPromoteRook );
            PromotionList.Add( blackPromoteBishop );
            PromotionList.Add( blackPromoteKnight );

        }

    }
}
