using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessClassLibrary;

namespace ChessUI
{
    public static class Images
    {

        private static readonly Dictionary<string, ImageSource> whiteSources = new()
        {
            { "pawn", LoadImage("Assets/PawnW.png")},
            { "rook", LoadImage("Assets/RookW.png")},
            { "bishop", LoadImage("Assets/BishopW.png")},
            { "knight", LoadImage("Assets/KnightW.png")},
            { "queen", LoadImage("Assets/QueenW.png")},
            { "king", LoadImage("Assets/KingW.png")}
        };

        private static readonly Dictionary<string, ImageSource> blackSources = new()
        {
            { "pawn", LoadImage("Assets/PawnB.png")},
            { "rook", LoadImage("Assets/RookB.png")},
            { "bishop", LoadImage("Assets/BishopB.png")},
            { "knight", LoadImage("Assets/KnightB.png")},
            { "queen", LoadImage("Assets/QueenB.png")},
            { "king", LoadImage("Assets/KingB.png")}
        };

        private static ImageSource LoadImage(string filepath)
        {
            Uri uri = new Uri(filepath, UriKind.Relative);
            return new BitmapImage(uri);
        }

        public static ImageSource GetImage(Piece piece)
        {
            string color = piece.Color;
            string name = piece.Name;

            switch(color)
            {
                case "white":
                    return whiteSources[name];
                case "black":
                    return blackSources[name];
                default:
                    return null;
            }
        }

        public static ImageSource GetHighlightImage(string type)
        {
            if(type == "empty")
            {
                return LoadImage( "Assets/Dot.png" );
            }
            else if (type == "capture")
            {
                return LoadImage( "Assets/CaptureRing.png" );
            }
            else
            {
                return null;
            }
        }

    }
}
