using Sevens.Game.Engine.Concrete;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sevens.Game.Engine.Interfaces
{
    public interface IHumanPlayer
    {
        IList<Card> ShowHand();

        void TakeTurn(Card? card, IGameState state);
    }
}
