using Sevens.Game.Engine.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sevens.Game.Engine.Models
{
    public class InGameSuit
    {
        public bool IsOpen { get; set; }
        public CardValue? HighValue { get; set; }
        public CardValue? LowValue { get; set; }
        public Suits Suit { get; }

        public InGameSuit(Suits suit)
        {
            Suit = suit;
            IsOpen = false;
        }
    }
}
