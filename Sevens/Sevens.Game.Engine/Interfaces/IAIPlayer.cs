using Sevens.Game.Engine.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sevens.Game.Engine.Interfaces
{
    public interface IAIPlayer
    {
        void TakeTurn(IGameState state);
    }
}
