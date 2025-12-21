using ChessClassLibrary;
using System.Buffers.Text;
using System.Drawing;
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
//En Passant
//Drawing Rules
//Castling Through Check/While Checked

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



namespace ChessUI
{

    public partial class MainWindow : Window
    {

        private readonly System.Windows.Controls.Image[,] pieceImages = new System.Windows.Controls.Image[8, 8];
        private Position selectedPosition = null;
        private Piece selectedPiece = null;
        Board board = new Board();



        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            //board.TestCastle();
            //board.TestSetup();
            board.SetupGame();
            DrawBoard(board);

        }


        private void InitializeBoard()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c  = 0; c < 8; c++)
                {
                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    pieceImages[r, c] = image;
                    PieceGrid.Children.Add(image);

                }
            }
        }

        public void DrawBoard(Board board)
        {
            for (int r = 0; r< 8 ; r++)
            {
                for (int c = 0; c < 8 ; c++)
                {
                    pieceImages[r, c].Source = null;
                }
            }
            foreach (var piece in board.Pieces)
            {
                int r = piece.Position.Row;
                int c = piece.Position.Column;

                pieceImages[r, c].Source = Images.GetImage(piece);
            }
        }

        public void DrawPromotionChoices(Board board)
        {
            foreach (var piece in board.PromotionList)
            {
                if (piece.Color == board.Turn)
                {
                    int r = piece.Position.Row;
                    int c = piece.Position.Column;

                    pieceImages[r, c].Source = Images.GetImage(piece);
                }
            }
        }

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);


            if (board.Promotion == true)
            {
                foreach(var piece in board.PromotionList)
                {
                    int posRow = pos.Row;
                    int posCol = pos.Column;
                    int pieceRow = piece.Position.Row;
                    int pieceCol = piece.Position.Column;

                    if (pieceRow == posRow && pieceCol == posCol)
                    {
                        foreach (var piece2 in board.Pieces)
                        {
                            if (piece2.Name == "pawn" && (piece2.Position.Row == 0 || piece2.Position.Row == 7))
                            {
                                int promotePositionRow = piece2.Position.Row;
                                int promotePositionCol = piece2.Position.Column;
                                board.Capture(promotePositionRow, promotePositionCol);
                                switch (piece.Name)
                                {
                                    case "queen":
                                        board.Pieces.Add(new Queen(piece.Name, piece.Color, promotePositionRow, promotePositionCol));
                                        break;
                                    case "rook":
                                        board.Pieces.Add(new Piece(piece.Name, piece.Color, promotePositionRow, promotePositionCol));
                                        break;
                                    case "bishop":
                                        board.Pieces.Add(new Piece(piece.Name, piece.Color, promotePositionRow, promotePositionCol));
                                        break;
                                    case "knight":
                                        board.Pieces.Add(new Piece(piece.Name, piece.Color, promotePositionRow, promotePositionCol));
                                        break;
                                    default:
                                        break;
                                }
                                DrawBoard(board);
                                board.NextTurn();
                                board.Promotion = false;
                                if (board.CheckForMate(board) == true)
                                {
                                    Checkmate.Visibility = Visibility.Visible;
                                }
                                selectedPosition = null;
                                selectedPiece = null;
                                return;
                            }
                        }
                    }
                }
                return;
            }



            if (selectedPosition == null)
            {
                OnFromPositionSelected(pos);
            }
            else
            {
                OnToPositionSelected(pos);
            }
        }

        private void OnFromPositionSelected(Position pos)
        {
            foreach (var piece in board.Pieces)
            {

                int posRow = pos.Row;
                int posCol = pos.Column;
                int pieceRow = piece.Position.Row;
                int pieceCol = piece.Position.Column;

                if (pieceRow == posRow && pieceCol == posCol)
                {
                    selectedPiece = piece;
                    selectedPosition = pos;
                    if (selectedPiece.Color != board.Turn)
                    {
                        selectedPiece = null;
                        selectedPosition = null;
                    }
                }
                
            }

        }

        private void OnToPositionSelected(Position pos)
        {
            if (selectedPiece != null)
            {
                selectedPiece.Move(board, pos);
                DrawBoard(board);


                if (board.Promotion == true)
                {
                    board.NextTurn();
                    DrawPromotionChoices(board);
                    board.Promotion = true;
                    return;
                }



                if (board.CheckForMate(board) == true)
                {
                    Checkmate.Visibility = Visibility.Visible;
                }

            }
            selectedPosition = null;
            selectedPiece = null;
        }


        private Position ToSquarePosition(System.Windows.Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);
            return new Position(row, col);

        }
        

    }
}
