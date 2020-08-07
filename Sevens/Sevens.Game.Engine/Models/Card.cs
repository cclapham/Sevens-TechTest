using Sevens.Game.Engine.Enums;
using Sevens.Game.Engine.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Sevens.Game.Engine.Models
{
    public class Card : IEquatable<Card>
    {
        public Suits Suit { get; }
        public CardValue Value { get; }

        public Card (Suits suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }

        public bool Equals([AllowNull] Card other)
        {
            if (this.Suit != other.Suit) return false;
            if (this.Value != other.Value) return false;

            return true;
        }

        public string GetFriendlyName()
        {
            return $"{this.Value.ToName()} of {this.Suit.ToName()}";
        }
    }
}
