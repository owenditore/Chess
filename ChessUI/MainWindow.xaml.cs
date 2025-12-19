using ChessClassLibrary;
using System.Buffers.Text;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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



        public MainWindow()
        {
            InitializeComponent();
            InitializeBoard();

            board.TestSetup();
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
                }
                
            }

        }

        private void OnToPositionSelected(Position pos)
        {
            if (selectedPiece != null)
            {
                selectedPiece.Move(board, pos);
                DrawBoard(board);
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
