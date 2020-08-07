using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Sevens.Game.Engine.Enums
{
    public enum Suits
    {
        [Description("H")]
        Hearts,
        [Description("S")]
        Spades,
        [Description("C")]
        Clubs,
        [Description("D")]
        Diamonds
    }

    public enum CardValue
    {
        [Description("A")]
        Ace = 1,
        [Description("2")]
        Two,
        [Description("3")]
        Three,
        [Description("4")]
        Four,
        [Description("5")]
        Five,
        [Description("6")]
        Six,
        [Description("7")]
        Seven,
        [Description("8")]
        Eight,
        [Description("9")]
        Nine,
        [Description("10")]
        Ten,
        [Description("J")]
        Jack,
        [Description("Q")]
        Queen,
        [Description("K")]
        King
    }
}
