using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Player
    {

        public List<Piece> Pieces { get; set; } = new List<Piece>();

        public string Color { get; set; } 

        public Player( string color, List<Piece> pieces ) 
        {
            this.Color = color;
            this.Pieces = pieces;
        }

        public Player( string Color )
        {
            this.Color = Color; 
        }

        public Piece ReturnPromotablePawn()
        {
            Piece? promotablePawn = this.Pieces.FirstOrDefault( p =>
                p.Name == "pawn" &&
                ( p.Position.Row == 7 ||
                p.Position.Row == 0 )
            );

            return promotablePawn;
        }

        public bool AnyPieceAllowedToMove( Board board )
        {
            foreach(Piece piece in this.Pieces)
            {
                if(piece.AllowedToMoveAnywhere( board ) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public void Capture( Position newPosition )
        {
            Pieces.RemoveAll( p => p.Position.IsEqual( newPosition ) );
        }

        public void Capture( Piece piece )
        {
            Pieces.RemoveAll( p => p.IsEqual( piece ) );
        }

        private Dictionary<string, int> FindNumberOfEachTypeOfPiece()
        {
            var numbersOfPieces = new Dictionary<string, int>
            {
                {"queen", this.Pieces.Count( p => p.Name == "queen" ) },
                {"rook", this.Pieces.Count( p => p.Name == "rook" ) },
                {"bishop", this.Pieces.Count( p => p.Name == "bishop") },
                {"knight", this.Pieces.Count( p => p.Name == "knight") },
                {"pawn", this.Pieces.Count( p => p.Name == "pawn" ) },
            };

            return numbersOfPieces;
        }

        public bool CheckForInsufficientMaterial()
        {
            var numbersOfPieces = FindNumberOfEachTypeOfPiece();

            if(PlayerHasAnyQueensRooksOrPawns( numbersOfPieces )) return false;

            if(PlayerHasTwoBishopsOrTwoKnights( numbersOfPieces )) return false;

            if(PlayerHasABishopAndAKnight( numbersOfPieces )) return false;

            return true;
        }

        private bool PlayerHasAnyQueensRooksOrPawns( Dictionary<string, int> numbersOfPieces )
        {

            int totalQRPs = numbersOfPieces["pawn"] + numbersOfPieces["rook"] + numbersOfPieces["queen"];

            if(totalQRPs != 0)
                return true;

            return false;
        }

        private bool PlayerHasTwoBishopsOrTwoKnights( Dictionary<string, int> numbersOfPieces )
        {

            if(numbersOfPieces["whiteKnight"] > 1) 
                return true;

            if(numbersOfPieces["whiteBishop"] > 1) 
                return true;

            return false;

        }

        private bool PlayerHasABishopAndAKnight( Dictionary<string, int> numbersOfPieces )
        {

            if(numbersOfPieces["whiteKnight"] + numbersOfPieces["whiteBishop"] > 1) 
                return true;


            return false;
        }

        public void SetupStartingPosition()
        {
            if(this.Color == "white")
            {
                this.WhiteStartingPosition();
            }
            if(this.Color == "black")
            {
                this.BlackStartingPosition();
            }
        }

        private void WhiteStartingPosition()
        {
            Pawn whitePawnA = new Pawn( "pawn", "white", 6, 0 );
            Pawn whitePawnB = new Pawn( "pawn", "white", 6, 1 );
            Pawn whitePawnC = new Pawn( "pawn", "white", 6, 2 );
            Pawn whitePawnD = new Pawn( "pawn", "white", 6, 3 );
            Pawn whitePawnE = new Pawn( "pawn", "white", 6, 4 );
            Pawn whitePawnF = new Pawn( "pawn", "white", 6, 5 );
            Pawn whitePawnG = new Pawn( "pawn", "white", 6, 6 );
            Pawn whitePawnH = new Pawn( "pawn", "white", 6, 7 );

            Rook whiteRookA = new Rook( "rook", "white", 7, 0 );
            Rook whiteRookH = new Rook( "rook", "white", 7, 7 );

            Knight whiteKnightB = new Knight( "knight", "white", 7, 1 );
            Knight whiteKnightG = new Knight( "knight", "white", 7, 6 );

            Bishop whiteBishopC = new Bishop( "bishop", "white", 7, 2 );
            Bishop whiteBishopF = new Bishop( "bishop", "white", 7, 5 );

            Queen whiteQueen = new Queen( "queen", "white", 7, 3 );
            King whiteKing = new King( "king", "white", 7, 4 );

            Pieces.Add( whitePawnA );
            Pieces.Add( whitePawnB );
            Pieces.Add( whitePawnC );
            Pieces.Add( whitePawnD );
            Pieces.Add( whitePawnE );
            Pieces.Add( whitePawnF );
            Pieces.Add( whitePawnG );
            Pieces.Add( whitePawnH );
            Pieces.Add( whiteRookA );
            Pieces.Add( whiteRookH );
            Pieces.Add( whiteKnightB );
            Pieces.Add( whiteKnightG );
            Pieces.Add( whiteBishopC );
            Pieces.Add( whiteBishopF );
            Pieces.Add( whiteQueen );
            Pieces.Add( whiteKing );

        }

        private void BlackStartingPosition()
        {
            Pawn blackPawnA = new Pawn( "pawn", "black", 1, 0 );
            Pawn blackPawnB = new Pawn( "pawn", "black", 1, 1 );
            Pawn blackPawnC = new Pawn( "pawn", "black", 1, 2 );
            Pawn blackPawnD = new Pawn( "pawn", "black", 1, 3 );
            Pawn blackPawnE = new Pawn( "pawn", "black", 1, 4 );
            Pawn blackPawnF = new Pawn( "pawn", "black", 1, 5 );
            Pawn blackPawnG = new Pawn( "pawn", "black", 1, 6 );
            Pawn blackPawnH = new Pawn( "pawn", "black", 1, 7 );

            Rook blackRookA = new Rook( "rook", "black", 0, 0 );
            Rook blackRookH = new Rook( "rook", "black", 0, 7 );

            Knight blackKnightB = new Knight( "knight", "black", 0, 1 );
            Knight blackKnightG = new Knight( "knight", "black", 0, 6 );

            Bishop blackBishopC = new Bishop( "bishop", "black", 0, 2 );
            Bishop blackBishopF = new Bishop( "bishop", "black", 0, 5 );

            Queen blackQueen = new Queen( "queen", "black", 0, 3 );
            King blackKing = new King( "king", "black", 0, 4 );

            Pieces.Add( blackPawnA );
            Pieces.Add( blackPawnB );
            Pieces.Add( blackPawnC );
            Pieces.Add( blackPawnD );
            Pieces.Add( blackPawnE );
            Pieces.Add( blackPawnF );
            Pieces.Add( blackPawnG );
            Pieces.Add( blackPawnH );
            Pieces.Add( blackRookA );
            Pieces.Add( blackRookH );
            Pieces.Add( blackKnightB );
            Pieces.Add( blackKnightG );
            Pieces.Add( blackBishopC );
            Pieces.Add( blackBishopF );
            Pieces.Add( blackQueen );
            Pieces.Add( blackKing );
        }



    }
}
