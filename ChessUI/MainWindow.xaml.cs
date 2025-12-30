using ChessClassLibrary;
using System.Buffers.Text;
using System.Drawing;
using System.IO.Pipelines;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



//TODO LIST

//Refactor to decrease method complexity

//3 Move Repition
//50 Move No Captures Or Pawn Advances



//Potential Game Experience Features
//Highlight your piece when it's clicked
//Highlight last move made in the game
//Show Valid Moves for the piece
//Show captures in a different color

//More Involved Features for a full application
//Main Menu before you start a game
//Make the board a subsection of a larger screen, include space for clocks or taken pieces
//Have clocks and different time controls
//Have different visual themes that can be changed
//Play as guests or as users, users would have game history and a login user/password to play as them, could have an elo system as well and play rated or unrated games
//Game history and ability to review past games

//Potential Bot features
//Make my own, would be very rudimentary
//Save the game in FEN or PGN format, pass to either local engine or web based engine to get back evaluation and best moves
//Evaluation bar either by my bot or by an engine on screen or in replay/analysis
//Can use https://chess-api.com/



namespace ChessUI
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            this.InitializeEmptyImagesForEachBoardSlot();

            //board.TestCastle();
            //board.TestSetup();
            board.SetupGame();

            this.DrawBoard();

        }

        private void AssignOccupiedSpacesToImages()
        {
            foreach(var piece in board.Pieces)
            {
                int row = piece.Position.Row;
                int column = piece.Position.Column;

                pieceImages[row, column].Source = Images.GetImage( piece );
            }
        }

        private void AttemptToMovePieceToNewPosition( Position newPosition )
        {
            if(selectedPiece != null)
            {
                selectedPiece.Move( board, newPosition );
                this.DrawBoard();

                if(board.NeedToPromote == true)
                {
                    this.DrawPromotionChoices( board );
                    return;
                }

                EndTurn();
            }
            else
            {
                this.DeselectPiece();
            }
        }

        private void BoardGrid_MouseDown( object sender, MouseButtonEventArgs e )
        {
            System.Windows.Point clickedPoint = e.GetPosition( BoardGrid );
            Position clickedPosition = this.ToSquarePosition( clickedPoint );

            if(board.NeedToPromote)
            {
                this.Promotion( clickedPosition );
            }

            else if(selectedPosition == null)
            {
                this.SetSelectedPositionAndPiece( clickedPosition );
            }

            else
            {
                this.AttemptToMovePieceToNewPosition( clickedPosition );
            }
        }

        private void CheckForEndOfGame()
        {
            string endOfGame = "";
            endOfGame = board.CheckForMateOrDraw();

            if(endOfGame == "checkmate")
            {
                Checkmate.Visibility = Visibility.Visible;
            }

            if(endOfGame == "draw")
            {
                Draw.Visibility = Visibility.Visible;
            }
        }

        private void ClearBoardImages()
        {
            for(int row = 0; row < 8; row++)
            {
                for(int column = 0; column < 8; column++)
                {
                    pieceImages[row, column].Source = null;
                }
            }
        }
        private void CreateImageForGridSlot( int row, int column )
        {
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            pieceImages[row, column] = image;
            PieceGrid.Children.Add( image );
        }

        private void DeselectPiece()
        {
            selectedPiece = null;
            selectedPosition = null;
        }

        private void DrawBoard()
        {
            this.ClearBoardImages();

            this.AssignOccupiedSpacesToImages();
        }

        private void DrawPromotionChoices( Board board )
        {
            foreach(var piece in board.PromotionList)
            {
                if(piece.Color == board.Turn)
                {
                    int row = piece.Position.Row;
                    int column = piece.Position.Column;

                    pieceImages[row, column].Source = Images.GetImage( piece );
                }
            }
        }

        private void EndTurn()
        {
            this.DrawBoard();
            board.NextTurn();

            this.CheckForEndOfGame();

            board.AddFEN( board );
            this.DeselectPiece();
        }

        private void InitializeEmptyImagesForEachBoardSlot()
        {
            for(int row = 0; row < 8; row++)
            {
                for(int column = 0; column < 8; column++)
                {
                    this.CreateImageForGridSlot( row, column );
                }
            }
        }

        private void Promotion( Position clickedPosition )
        {
            Piece? promotablePiece = board.CheckForPromotablePiece();
            Piece? promoteToPiece = board.CheckForPieceToPromoteTo( clickedPosition );

            if(promotablePiece != null && promoteToPiece != null)
            {

                board.PromotePiece( promotablePiece, promoteToPiece );
                board.NeedToPromote = false;
                this.EndTurn();

            }
        }

        private void SetPositionAndPieceIfCorrectTurn( Piece piece, Position clickedPosition )
        {
            selectedPiece = piece;
            selectedPosition = clickedPosition;

            if(selectedPiece.Color != board.Turn)
            {
                this.DeselectPiece();
            }

        }

        private void SetSelectedPositionAndPiece( Position clickedPosition )
        {
            foreach(var piece in board.Pieces)
            {

                if(piece.Position.IsEqual( clickedPosition ))
                {
                    this.SetPositionAndPieceIfCorrectTurn( piece, clickedPosition );
                }
            }
        }

        private Position ToSquarePosition( System.Windows.Point point )
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)( point.Y / squareSize );
            int col = (int)( point.X / squareSize );
            return new Position( row, col );

        }

        private readonly System.Windows.Controls.Image[,] pieceImages = new System.Windows.Controls.Image[8, 8];
        Board board = new Board();
        private Piece selectedPiece = null;
        private Position selectedPosition = null;
    }
}
