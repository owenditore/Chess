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
using ChessClassLibrary;

namespace ChessUI
{

    public partial class MainWindow : Window
    {
        Position ReferencePosition { get; } = new Position(-1, -1);
        Position LastClickPosition { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Board board = new Board();
            board.TestSetup();
            DisplayPieces(board);
            do
            {
                do
                {
                    if (LastClickPosition != ReferencePosition)
                    {

                    }

                } while (LastClickPosition == ReferencePosition);

                Piece returnedPiece = board.WhatPieceIsHere(newPosition);
                //Remove piece if there was a capture
                //check Check/CheckMate
                //RefreshDisplay
            } while (board.CheckMate != true);

        }

        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            string gridData = (string)((FrameworkElement)sender).DataContext;
            char rowC = (char)gridData[0];
            char colC = (char)gridData[1];
            Position position = new Position((int)rowC,(int)colC);
            LastClickPosition = position;

            //return position;
        }

        private void DisplayPieces(Board board)
        {
            foreach (var piece in board.Pieces)
            {
                //Put the corresponding image at the corresponding position on grid.
            }
        }
    }
}