using Sevens.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sevens.Game.Engine.Interfaces
{
    public interface IDeck
    {
        void Shuffle();

        void Deal(ref IList<IPlayer> players);
    }
}
