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
    public class PlayerBase : IPlayer
    {
        protected IGameBoard _board;

        protected IList<Card> _hand;

        public PlayerBase(IGameBoard board)
        {
            _board = board;
            _hand = new List<Card>();
        }

        public void UpdateHand(Card card, bool isPlayed)
        {
            if(isPlayed)
            {
                RemoveCard(card);
            }
            else
            {
                AddCard(card);
            }
        }

        private void RemoveCard(Card card)
        {
            _hand.Remove(card);
        }

        private void AddCard(Card card)
        {
            _hand.Add(card);
        }

        public bool HasPlayerWon()
        {
            if (_hand.Count == 0)
                return true;

            return false;
        }

        public bool HasSevenDiamonds()
        {
            var sevenD = new Card(Enums.Suits.Diamonds, Enums.CardValue.Seven);

            if (_hand.Contains(sevenD))
                return true;
            return false;
        }

        public void FirstTurn(IGameState state)
        {
            // If first turn check if can play
            if (_board.IsFirstTurn())
            {
                if (HasSevenDiamonds())
                {
                    //Play the seven of diamonds
                    var card = new Card(Suits.Diamonds, CardValue.Seven);

                    _board.SetGameBoardState(card);
                    UpdateHand(card, true);

                    var move = new MoveHistory
                    {
                        Player = this,
                        move = card
                    };

                    state.AddMove(move);
                    return;
                }
            }
        }

        public bool UnableToPlay()
        {
            return _hand.Any(x => _board.IsEligible(x));
        }

        public virtual bool IsHuman()
        {
            return false;
        }

        public virtual string Name()
        {
            throw new NotImplementedException();
        }
    }
}
