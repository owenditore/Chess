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
//Highlight last move made in the game


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

        }

        private void Local_Click( object sender, RoutedEventArgs e )
        {
            HideLocalOnlineMenu();

            this.InitializeBoardGraphics();

            board.SetupGame();

            this.DrawBoard();
        }

        private void Online_Click( object sender, RoutedEventArgs e )
        {
            HideLocalOnlineMenu();
            RevealNewOrReturningMenu();
        }

        private void NewUser_Click( object sender, RoutedEventArgs e )
        {
            HideNewOrReturningMenu();
            RevealNewUserEnterInfo();
            newUser = true;
        }

        private void Enter_Click( object sender, RoutedEventArgs e )
        {
            username = UsernameEntryBox.Text;
            userID = databaseConnection.LoginUser( newUser, username );

            if(userID == 0)
            {
                InvalidUsername.Visibility = Visibility.Visible;
                return;
            }

            HideUserEnterInfo();
            RevealNewOrResumeGame();
        }

        private void NewGame_Click( object sender, RoutedEventArgs e )
        {
            HideNewOrResumeGame();
            users = databaseConnection.GetDictionaryOfUsers();
            DisplayUserList();
        }

        private void ResumeGame_Click( object sender, RoutedEventArgs e )
        {
            HideNewOrResumeGame();
        }

        private void ListOfUsers_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            if(ListOfUsers.SelectedItem == null)
            {
                return;
            }
            string selectedUser = ListOfUsers.SelectedItem.ToString();
            opponentUserID = users[selectedUser];

            HideUserList();

            gameID = databaseConnection.CreateNewGame( userID, opponentUserID );

            playerColor = "white";

            this.InitializeBoardGraphics();

            board.SetupGame();

            this.DrawBoard();

        }

        private void HideUserList()
        {
            ListOfUsers.Visibility = Visibility.Collapsed;
            SelecteWhoYouWantToPlay.Visibility = Visibility.Collapsed;
        }

        private void DisplayUserList()
        {
            List<string> usernames = users.Keys.ToList();
            usernames.Remove( username );
            ListOfUsers.ItemsSource = usernames;
            ListOfUsers.Visibility = Visibility.Visible;
            SelecteWhoYouWantToPlay.Visibility = Visibility.Visible;
        }

        private void HideNewOrResumeGame()
        {
            NewGame.Visibility = Visibility.Collapsed;
            ResumeGame.Visibility = Visibility.Collapsed;
        }

        private void RevealNewOrResumeGame()
        {
            NewGame.Visibility = Visibility.Visible;
            ResumeGame.Visibility = Visibility.Visible;
        }

        private void Returning_Click( object sender, RoutedEventArgs e )
        {
            HideNewOrReturningMenu();
            RevealReturningUserEnterInfo();
            newUser = false;
        }

        private void RevealReturningUserEnterInfo()
        {
            ReturningUserEnterUsername.Visibility = Visibility.Visible;
            UsernameEntryBox.Visibility = Visibility.Visible;
            Enter.Visibility = Visibility.Visible;
        }

        private void HideUserEnterInfo()
        {
            NewUserEnterUsername.Visibility = Visibility.Collapsed;
            ReturningUserEnterUsername.Visibility = Visibility.Collapsed;
            UsernameEntryBox.Visibility = Visibility.Collapsed;
            Enter.Visibility = Visibility.Collapsed;
            InvalidUsername.Visibility = Visibility.Collapsed;
        }

        private void RevealNewUserEnterInfo()
        {
            NewUserEnterUsername.Visibility = Visibility.Visible;
            UsernameEntryBox.Visibility = Visibility.Visible;
            Enter.Visibility = Visibility.Visible;
        }

        private void HideNewOrReturningMenu()
        {
            ReturningUser.Visibility = Visibility.Collapsed;
            NewUser.Visibility = Visibility.Collapsed;
        }

        private void RevealNewOrReturningMenu()
        {
            ReturningUser.Visibility = Visibility.Visible;
            NewUser.Visibility = Visibility.Visible;
        }

        private void HideLocalOnlineMenu()
        {
            Local.Visibility = Visibility.Collapsed;
            Online.Visibility = Visibility.Collapsed;
            PlayLocalOrOnline.Visibility = Visibility.Collapsed;
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

        private void AssignHighlights()
        {
            foreach(var square in board.Squares)
            {
                if(selectedPiece.AllowedToMove( board, square.Position ))
                {
                    DetermineAndDisplayHighlight( square );
                }
            }
        }

        private void DetermineAndDisplayHighlight( Square square )
        {
            if(square.Piece != null)
            {
                highlights[square.Position.Row, square.Position.Column].Source = Images.GetHighlightImage( "capture" );
            }
            else
            {
                highlights[square.Position.Row, square.Position.Column].Source = Images.GetHighlightImage( "empty" );
            }
        }

        private void AttemptToMovePieceToNewPosition( Position newPosition )
        {
            if(selectedPiece.AllowedToMove( board, newPosition ) == false)
            {
                this.DeselectPiece();
                this.ClearBoardHighlights();
                return;
            }

            selectedPiece.MovePiece( board, newPosition );
            this.DrawBoard();

            if(board.APawnNeedsToPromote == true)
            {
                this.DrawPromotionChoices( board );
                return;
            }

            EndTurn();

        }

        private void BoardGrid_MouseDown( object sender, MouseButtonEventArgs e )
        {
            System.Windows.Point clickedPoint = e.GetPosition( BoardGrid );
            Position clickedPosition = this.ToSquarePosition( clickedPoint );

            if(board.APawnNeedsToPromote)
            {
                this.Promotion( clickedPosition );
            }

            else if(selectedPiece == null)
            {
                this.SetSelectedPositionAndPiece( clickedPosition );
            }

            else
            {
                this.AttemptToMovePieceToNewPosition( clickedPosition );
            }
        }

        private string CheckForEndOfGame()
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
            return endOfGame;
        }

        private void ClearBoardHighlights()
        {
            for(int row = 0; row < 8; row++)
            {
                for(int column = 0; column < 8; column++)
                {
                    highlights[row, column].Source = null;
                }
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
        private void CreatePieceImageForGridSlot( int row, int column )
        {
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            pieceImages[row, column] = image;
            PieceGrid.Children.Add( image );
        }

        private void CreateHighlightImageForGridSlot( int row, int column )
        {
            System.Windows.Controls.Image highlight = new System.Windows.Controls.Image();
            highlights[row, column] = highlight;
            HighlightGrid.Children.Add( highlight );
        }

        private void DeselectPiece()
        {
            selectedPiece = null;
        }

        private void DrawBoard()
        {
            this.ClearBoardImages();
            this.ClearBoardHighlights();

            this.AssignOccupiedSpacesToImages();
        }

        private void DrawPromotionChoices( Board board )
        {
            foreach(var piece in board.PromotionList)
            {
                if(piece.Color == board.CurrentTurnColor)
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

            string endOfGame = this.CheckForEndOfGame();
            this.DeselectPiece();

            board.AddFEN();
            fen = board.GetCurrentFEN();

            string stateOfGame = "In Progress";
            if(endOfGame != "")
            {
                stateOfGame = "Completed";
            }

            if(gameID != 0)
            {
                databaseConnection.EndTurnUpdateGame( gameID, fen, stateOfGame );
            }
        }

        private void InitializeBoardGraphics()
        {
            for(int row = 0; row < 8; row++)
            {
                for(int column = 0; column < 8; column++)
                {
                    this.CreatePieceImageForGridSlot( row, column );
                    this.CreateHighlightImageForGridSlot( row, column );
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
                board.APawnNeedsToPromote = false;
                this.EndTurn();

            }
        }

        private void SetPositionAndPieceIfCorrectTurn( Piece piece, Position clickedPosition )
        {
            selectedPiece = piece;

            if(selectedPiece.Color != board.CurrentTurnColor)
            {
                this.DeselectPiece();
                return;
            }

            if(gameID != 0)
            {
                if(selectedPiece.Color != playerColor)
                {
                    this.DeselectPiece();
                    return;
                }
            }

            this.AssignHighlights();
        }

        private void SetSelectedPositionAndPiece( Position clickedPosition )
        {
            foreach(var piece in board.Pieces)
            {
                if(piece.Position.IsEqual( clickedPosition ))
                {
                    this.SetPositionAndPieceIfCorrectTurn( piece, clickedPosition );
                    return;
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
        private readonly System.Windows.Controls.Image[,] highlights = new System.Windows.Controls.Image[8, 8];

        string fen;
        string playerColor;
        Dictionary<string, int> users;
        int gameID = 0;
        string username;
        bool newUser;
        int userID = 0;
        int opponentUserID = 0;
        Board board = new Board();
        DatabaseConnection databaseConnection = new DatabaseConnection();
        private Piece selectedPiece = null;
    }
}
