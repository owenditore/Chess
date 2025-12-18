using System;
using System.Collections.Generic;
using System.Text;

namespace ChessClassLibrary
{
    public class Turn
    {
        public string TurnColor { get; private set; } = "white";

        public void NextTurn()
        {
            if (TurnColor == "white")
                TurnColor = "black";
            else
                TurnColor = "white";
        }

    }
}
