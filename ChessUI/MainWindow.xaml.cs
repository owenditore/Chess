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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            string gridData = (string)((FrameworkElement)sender).DataContext;
        }

        public void RefreshDisplay(List<Piece> pieces)
        {
            //Update The Board based on each Piece in list pieces.
        }

    }
}