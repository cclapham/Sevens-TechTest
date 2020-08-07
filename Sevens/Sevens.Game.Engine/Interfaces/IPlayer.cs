using Sevens.Game.Engine.Concrete;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;

namespace Sevens.Game.Engine.Interfaces
{
    public interface IPlayer
    {
        void UpdateHand(Card card, bool isPlayed);

        bool HasPlayerWon();

        bool HasSevenDiamonds();
        void FirstTurn(IGameState state);

        bool UnableToPlay();

        bool IsHuman();

        string Name();
    }
}
