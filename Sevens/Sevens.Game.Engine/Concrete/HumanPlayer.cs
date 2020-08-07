using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sevens.Game.Engine.Concrete
{
    public class HumanPlayer : PlayerBase, IHumanPlayer
    {
        public HumanPlayer(IGameBoard board) : base(board) { }
        public IList<Card> ShowHand()
        {
            // Order the hand for display
            var hand = new List<Card>();

            hand.AddRange(_hand.Where(x => x.Suit == Enums.Suits.Diamonds).OrderBy(x => x.Value));
            hand.AddRange(_hand.Where(x => x.Suit == Enums.Suits.Spades).OrderBy(x => x.Value));
            hand.AddRange(_hand.Where(x => x.Suit == Enums.Suits.Hearts).OrderBy(x => x.Value));
            hand.AddRange(_hand.Where(x => x.Suit == Enums.Suits.Clubs).OrderBy(x => x.Value));

            return hand;
        }

        public void TakeTurn(Card? card, IGameState state)
        {
            if (state == null) throw new ArgumentNullException("state");

            var move = new MoveHistory
            {
                Player = this
            };

            if (card != null)
            {
                _board.SetGameBoardState(card);
                UpdateHand(card, true);
                move.move = card;
            }

            state.AddMove(move);
        }

        public override bool IsHuman()
        {
            return true;
        }

        public override string Name()
        {
            return "You";
        }
    }
}
