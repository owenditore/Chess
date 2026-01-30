using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class GameLog
    {

        public int GameID { get; set; }

        public int WhitePlayerID { get; set; }

        public int BlackPlayerID { get; set; }

        public string FEN {  get; set; }

        public string StateOfGame { get; set; }

        public string CurrentMoveColor { get; set; }

        public GameLog( int gameID, int whitePlayerID, int blackPlayerID, string fen, string stateOfGame )
        {
            this.GameID = gameID;
            this.WhitePlayerID = whitePlayerID;
            this.BlackPlayerID = blackPlayerID;
            this.FEN = fen;
            this.StateOfGame = stateOfGame;
            SetCurrentMoveColor( fen );
        }

        private void SetCurrentMoveColor( string fen )
        {
            string[] partsOfFen = fen.Split( ' ' );
            string colorLetter = partsOfFen[1];

            if(colorLetter == "w")
            {
                CurrentMoveColor = "white";
            }
            else
            {
                CurrentMoveColor = "black";
            }
        }

    }
}
