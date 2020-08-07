using Sevens.Game.Engine.Enums;
using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sevens.Game.Engine.Concrete
{
    public class Deck : IDeck
    {
        private Stack<Card> _deck;

        public Deck()
        {
            _deck = new Stack<Card>();
        }

        public void Deal(ref IList<IPlayer> players)
        {
            if (players == null) throw new ArgumentNullException("players");

            int counter = 0;
            int totalPlayers = players.Count;
            while(_deck.TryPop(out Card card))
            {
                players.ElementAt(counter++ % totalPlayers).UpdateHand(card, false);
            }
        }

        public void Shuffle()
        {
            var deck = CreateDeck();
            //Generate a random number starting between 0 and 51 to select a card at random then reduce bounds until max is 0.
            Random rand = new Random();
            for(int i = 51; i >= 0; i--)
            {
                int cardNum = rand.Next(0, i);
                var card = deck.ElementAt(cardNum);
                _deck.Push(card);
                deck.Remove(card);
            }
        }


        private List<Card> CreateDeck()
        {
            // Add cards in suit (d c h s) order A-K
            List<Card> response = new List<Card>();
            response.AddRange(CreateSuit(Enums.Suits.Diamonds));
            response.AddRange(CreateSuit(Enums.Suits.Clubs));
            response.AddRange(CreateSuit(Enums.Suits.Hearts));
            response.AddRange(CreateSuit(Enums.Suits.Spades));

            return response;
        }

        private List<Card> CreateSuit(Suits suit)
        {
            var newSuit = new List<Card>();

            newSuit.Add(new Card(suit,CardValue.Ace));
            newSuit.Add(new Card(suit, CardValue.Two));
            newSuit.Add(new Card(suit, CardValue.Three));
            newSuit.Add(new Card(suit, CardValue.Four));
            newSuit.Add(new Card(suit, CardValue.Five));
            newSuit.Add(new Card(suit, CardValue.Six));
            newSuit.Add(new Card(suit, CardValue.Seven));
            newSuit.Add(new Card(suit, CardValue.Eight));
            newSuit.Add(new Card(suit, CardValue.Nine));
            newSuit.Add(new Card(suit, CardValue.Ten));
            newSuit.Add(new Card(suit, CardValue.Jack));
            newSuit.Add(new Card(suit, CardValue.Queen));
            newSuit.Add(new Card(suit, CardValue.King));

            return newSuit;
        }
    }
}
