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

namespace ChessUI
{

    public partial class MainWindow : Window
    {

        private readonly System.Windows.Controls.Image[,] pieceImages = new System.Windows.Controls.Image[8, 8];
        private Position selectedPosition = null;
        private Piece selectedPiece = null;
        Board board = new Board();

        //private List<UIElement> borders = new List<UIElement>();



        public MainWindow()
        {
            InitializeComponent();
            //InitializeBorders();
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

        private void DrawBoard(Board board)
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

        private void BoardGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(BoardGrid);
            Position pos = ToSquarePosition(point);

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
                    //HighlightPosition(selectedPosition);
                }
                
            }

        }

        private void OnToPositionSelected(Position pos)
        {
            if (selectedPiece != null)
            {
                selectedPiece.Move(board, pos);
                DrawBoard(board);

                if (board.CheckForMate(board) == true)
                {
                    Checkmate.Visibility = Visibility.Visible;
                }

            }

            selectedPosition = null;
            selectedPiece = null;
            //clearHighlights();
        }


        private Position ToSquarePosition(System.Windows.Point point)
        {
            double squareSize = BoardGrid.ActualWidth / 8;
            int row = (int)(point.Y / squareSize);
            int col = (int)(point.X / squareSize);
            return new Position(row, col);

        }

        /*
        private void HighlightPosition(Position position)
        {

        }

        private void clearHighlights()
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    foreach (UIElement border in borders)
                    {
                        border.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void intializeBorders()
        {
            borders.Add(S00);
            borders.Add(S01);
            borders.Add(S02);
            borders.Add(S03);
            borders.Add(S04);
            borders.Add(S05);
            borders.Add(S06);
            borders.Add(S07);

            borders.Add(S10);
            borders.Add(S11);
            borders.Add(S12);
            borders.Add(S13);
            borders.Add(S14);
            borders.Add(S15);
            borders.Add(S16);
            borders.Add(S17);

            borders.Add(S20);
            borders.Add(S21);
            borders.Add(S22);
            borders.Add(S23);
            borders.Add(S24);
            borders.Add(S25);
            borders.Add(S26);
            borders.Add(S27);

            borders.Add(S30);
            borders.Add(S31);
            borders.Add(S32);
            borders.Add(S33);
            borders.Add(S34);
            borders.Add(S35);
            borders.Add(S36);
            borders.Add(S37);

            borders.Add(S40);
            borders.Add(S41);
            borders.Add(S42);
            borders.Add(S43);
            borders.Add(S44);
            borders.Add(S45);
            borders.Add(S46);
            borders.Add(S47);

            borders.Add(S50);
            borders.Add(S51);
            borders.Add(S52);
            borders.Add(S53);
            borders.Add(S54);
            borders.Add(S55);
            borders.Add(S56);
            borders.Add(S57);

            borders.Add(S60);
            borders.Add(S61);
            borders.Add(S62);
            borders.Add(S63);
            borders.Add(S64);
            borders.Add(S65);
            borders.Add(S66);
            borders.Add(S67);

            borders.Add(S70);
            borders.Add(S71);
            borders.Add(S72);
            borders.Add(S73);
            borders.Add(S74);
            borders.Add(S75);
            borders.Add(S76);
            borders.Add(S77);

        }
        */
        

    }
}
