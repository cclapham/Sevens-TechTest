using Sevens.Game.Engine.Interfaces;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sevens.Game.Engine.Concrete
{
    public class UnSkilledPlayer : PlayerBase, IAIPlayer
    {
        public UnSkilledPlayer(IGameBoard board) : base(board)
        {
        }

        public void TakeTurn(IGameState state)
        {
            var move = new MoveHistory
            {
                Player = this
            };
            foreach(var card in _hand)
            {
                if(_board.IsEligible(card))
                {
                    //Play the card
                    _board.SetGameBoardState(card);
                    UpdateHand(card, true);

                    move.move = card;
                    state.AddMove(move);
                    return;
                }
            }

            state.AddMove(move);
        }

        public override string Name()
        {
            return "Unskilled AI";
        }
    }
}
