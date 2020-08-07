using Sevens.Game.Engine.Enums;
using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sevens.Game.Engine.Interfaces
{
    public interface IGameBoard
    {
        public IDictionary<Suits, InGameSuit> GetGameBoardState();

        public void SetGameBoardState(Card card);

        public IDictionary<Suits, InGameSuit> ResetGameBoardState();

        public bool IsEligible(Card card);

        public bool IsFirstTurn();
    }
}
