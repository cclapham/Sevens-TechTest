using Sevens.Game.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sevens.Game.Engine.Models
{
    public class MoveHistory
    {
        public IPlayer Player { get; set; }

        public Card? move { get; set; }
    }
}
